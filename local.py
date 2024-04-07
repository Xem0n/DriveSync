import time

from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler

class LocalFileHandler(FileSystemEventHandler):
    def __init__(self, sync_manager, debug):
        self.sync_manager = sync_manager
        self.__debug = debug

    def on_modified(self, event):
        if event.is_directory:
            return

        if self.__debug:
            print(f'event type: {event.event_type}  path : {event.src_path}')

        self.sync_manager.upload()

def init(file_path, sync_manager, debug):
    event_handler = LocalFileHandler(sync_manager, debug)

    observer = Observer()
    observer.schedule(event_handler, path=file_path, recursive=False)
    observer.start()

    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        observer.stop()

    observer.join()
