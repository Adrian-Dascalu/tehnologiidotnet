import json
import sys

def process_json_file():
    try:
        # Read the data-raw-dump.json file
        print("Reading data-raw-dump.json...")
        with open('data-raw-dump.json', 'r', encoding='utf-8') as f:
            data = json.load(f)
        
        # Check if 'recipe' key exists
        if 'recipe' in data:
            print("Found 'recipe' key in the JSON file")
            recipes = data['recipe']
            
            # Create recipe.json with the extracted recipes
            print("Creating recipe.json...")
            with open('recipe.json', 'w', encoding='utf-8') as f:
                json.dump(recipes, f, indent=2, ensure_ascii=False)
            print("Successfully created recipe.json")
        else:
            print("Warning: No 'recipe' key found in the JSON file")
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
