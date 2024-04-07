import subprocess
import uuid
import webhook_server
import local

from file_sync_manager import FileSyncManager
from argparse import ArgumentParser
from threading import Thread

parser = ArgumentParser(prog="DriveSync")
parser.add_argument("-p", "--port", type=int, default=3999, help="Port to run the webhook server on")
parser.add_argument("-r", "--resource", type=str, help="ID of the Google Drive's file to watch for changes")
parser.add_argument("-f", "--file", type=str, help="Path to the local file to sync with Google Drive")
parser.add_argument("-d", "--debug", action="store_true", default=False, help="Enable debug mode")

# todo: make it more readable

def main():
    args = parser.parse_args()

    validate_args(args)

    drive_sync_id = str(uuid.uuid4())
    sync_manager = FileSyncManager(args.file, args.resource, args.debug)

    run_localtunnel(args.port, drive_sync_id, args.debug)
    run_webhook_server(args.port, sync_manager, args.debug)
    run_local_file_handler(args.file, sync_manager, args.debug)

    sync_manager.download(force=True)
    sync_manager.watch(get_webhook_url(drive_sync_id))

    print("DriveSync is running...")

def run_localtunnel(port, subdomain, debug):
    stdout = subprocess.DEVNULL if not debug else None

    Thread(
        target=subprocess.run,
        args=[["lt", "--port", str(port), "--subdomain", subdomain]],
        kwargs={"stdout": stdout}
    ).start()

def run_webhook_server(port, sync_manager, debug):
    Thread(target=webhook_server.init, args=[port, sync_manager, debug]).start()

def run_local_file_handler(file_path, sync_manager, debug):
    Thread(target=local.init, args=[file_path, sync_manager, debug]).start()

def get_webhook_url(service_id):
    return f"https://{service_id}.loca.lt/notifications"

# todo: implement
def validate_args(args):
    pass

if __name__ == "__main__":
    main()
