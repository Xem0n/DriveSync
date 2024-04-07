import uuid
import dateparser
import mimetypes
import os.path

from time import time
from functools import wraps
from google.auth.transport.requests import Request
from google.oauth2.credentials import Credentials
from google_auth_oauthlib.flow import InstalledAppFlow
from googleapiclient.discovery import build
from googleapiclient.errors import HttpError
from googleapiclient.http import MediaFileUpload

DAY = 86400
# If modifying these scopes, delete the file token.json.
SCOPES = ["https://www.googleapis.com/auth/drive"]
INTERVAL = 15

def avoid_loop(func):
    @wraps(func)
    def wrapper(self, *args, **kwargs):
        force = kwargs.get("force", False)

        if not force and self.last_event + INTERVAL > time():
            return

        func(self, *args, **kwargs)

        self.last_event = time()

    return wrapper

class FileSyncManager:
    def __init__(self, file_path, resource_id, debug):
        creds = self.__get_credentials()

        self.service = build("drive", "v3", credentials=creds)
        self.file_path = file_path
        self.resource_id = resource_id
        self.last_event = 0

        self.__debug = debug

    def __get_credentials(self):
        creds = None

        if os.path.exists("token.json"):
            creds = Credentials.from_authorized_user_file("token.json", SCOPES)

        if not creds or not creds.valid:
            if creds and creds.expired and creds.refresh_token:
                creds.refresh(Request())
            else:
                flow = InstalledAppFlow.from_client_secrets_file(
                    "credentials.json", SCOPES
                )
                creds = flow.run_local_server(port=0)

            with open("token.json", "w") as token:
                token.write(creds.to_json())

        return creds

    def watch(self, webhook_url):
        channel_id = str(uuid.uuid4())
        body = {
            "id": channel_id,
            "type": "web_hook",
            "address": webhook_url,
            "expiration": int(time() + DAY) * 1000,
        }

        response = self.service.files().watch(fileId=self.resource_id, body=body).execute()
        ts = response["expiration"]

        if self.__debug:
            print(dateparser.parse(ts))
            print(response)

        return channel_id

    @avoid_loop
    def download(self, *, force=False):
        response = self.service.files().get_media(fileId=self.resource_id).execute()

        with open(self.file_path, "wb") as f:
            f.write(response)

        if self.__debug:
            print("file downloaded")

    @avoid_loop
    def upload(self, *, force=False):
        mime = mimetypes.guess_type(self.file_path, strict=False)[0]
        media = MediaFileUpload(self.file_path, mimetype=mime)
        response = self.service.files().update(fileId=self.resource_id, media_body=media).execute()

        if self.__debug:
            print("file uploaded")
