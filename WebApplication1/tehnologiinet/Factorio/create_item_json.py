import json
import sys

def process_json_file():
    try:
        # Read the data-raw-dump.json file
        print("Reading data-raw-dump.json...")
        with open('data-raw-dump.json', 'r', encoding='utf-8') as f:
            data = json.load(f)
        
        # Check if 'item' key exists
        if 'item' in data:
            print("Found 'item' key in the JSON file")
            items = data['item']
            
            # Create item.json with the extracted items
            print("Creating item.json...")
            with open('item.json', 'w', encoding='utf-8') as f:
                json.dump(items, f, indent=2, ensure_ascii=False)
            print("Successfully created item.json")
        else:
            print("Warning: No 'item' key found in the JSON file")
            print("Available top-level keys:", list(data.keys()))
            
    except FileNotFoundError:
        print("Error: data-raw-dump.json not found")
        sys.exit(1)
    except json.JSONDecodeError:
        print("Error: Invalid JSON format in data-raw-dump.json")
        sys.exit(1)
    except Exception as e:
        print(f"An error occurred: {str(e)}")
        sys.exit(1)

if __name__ == "__main__":
    process_json_file()
