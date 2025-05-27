#read from production-nauvis-10min.json and remove the last comma from each item in the list
import json
import re
import os
# Set the working directory to the location of the script
os.chdir(os.path.dirname(os.path.abspath(__file__)))
# Get the current working directory
current_directory = os.getcwd()

file_path = os.path.join(current_directory, 'production-nauvis-10min.json')

with open(file_path, 'r') as f:
    data = json.load(f)

# Iterate through each item in the list and remove the last comma
for item in data:
    if isinstance(item, dict):
        for key, value in item.items():
            if isinstance(value, str):
                # Remove the last comma from the string
                item[key] = re.sub(r',\s*$', '', value)
            elif isinstance(value, list):
                # Remove the last comma from each string in the list
                item[key] = [re.sub(r',\s*$', '', str(v)) for v in value]
            elif isinstance(value, dict):
                # Remove the last comma from each string in the dictionary
                for k, v in value.items():
                    if isinstance(v, str):
                        value[k] = re.sub(r',\s*$', '', v)
                    elif isinstance(v, list):
                        value[k] = [re.sub(r',\s*$', '', str(val)) for val in v]
                    elif isinstance(v, dict):
                        value[k] = {kk: re.sub(r',\s*$', '', str(val)) for kk, val in v.items() if isinstance(val, str) or isinstance(val, list) or isinstance(val, dict)}
                    else:
                        value[k] = v
            else:
                item[key] = value

#save the modified data back to the file
with open('production-nauvis-10min_rem.json', 'w') as f:
    json.dump(data, f, indent=4)

