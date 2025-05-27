import time
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler
import subprocess
import os

class JsonFileHandler(FileSystemEventHandler):
    def __init__(self):
        self.last_modified = 0
        self.processing = False

    def on_modified(self, event):
        # Only process if it's the data-raw-dump.json file
        if event.src_path.endswith('data-raw-dump.json'):
            # Get current timestamp
            current_time = time.time()
            
            # Prevent multiple processing within 1 second
            if current_time - self.last_modified < 1:
                return
                
            self.last_modified = current_time
            
            # Prevent concurrent processing
            if self.processing:
                return
                
            self.processing = True
            print("\nDetected change in data-raw-dump.json")
            
            try:
                # Run create_item_json.py
                print("\nRunning create_item_json.py...")
                subprocess.run(['python', 'create_item_json.py'], check=True)
                
                # Run create_recipe_json.py
                print("\nRunning create_recipe_json.py...")
                subprocess.run(['python', 'create_recipe_json.py'], check=True)
                
                print("\nProcessing completed successfully")
            except subprocess.CalledProcessError as e:
                print(f"\nError running scripts: {e}")
            finally:
                self.processing = False

def main():
    # Get the current directory
    path = os.getcwd()
    
    # Create an observer and handler
    event_handler = JsonFileHandler()
    observer = Observer()
    observer.schedule(event_handler, path, recursive=False)
    
    print(f"Starting to watch {os.path.join(path, 'data-raw-dump.json')}")
    print("Press Ctrl+C to stop...")
    
    # Start the observer
    observer.start()
    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        observer.stop()
        print("\nStopping file watcher...")
    observer.join()

if __name__ == "__main__":
    main() 