
import os

def write_part(filename, content):
    with open(filename, 'w', encoding='utf-8') as f:
        f.writelines(content)
    print(f"Created {filename}")

def split_file(source_path, ranges, type_suffix):
    with open(source_path, 'r', encoding='utf-8') as f:
        lines = f.readlines()
    
    for folder, start, end in ranges:
        # Convert 1-based index to 0-based
        # Start is inclusive, End is inclusive logic in my notes, but array slicing is [start:end] where end is exclusive
        # So I need to verify my ranges.
        # Range [1, 290] -> lines[0:290]
        # Range [291, 423] -> lines[290:423]
        
        # Adjust 1-based start to 0-based index
        s_idx = start - 1
        
        # Adjust 1-based end to 0-based exclusive index
        # If end is -1, it means until the end
        if end == -1:
            part_lines = lines[s_idx:]
        else:
            part_lines = lines[s_idx:end]
            
        target_dir = f"notes/readable/{folder}"
        target_file = f"{target_dir}/{folder}_{type_suffix}.md"
        
        if not os.path.exists(target_dir):
            os.makedirs(target_dir)
            
        write_part(target_file, part_lines)

# Define ranges (Start Line, End Line Inclusive)
# Note: In Python slicing [x:y], y is exclusive. 
# So if I want line 290 included, I need slice[0:290] -> 290 items (indices 0 to 289). Correct.
# My ranges:
# 1. 1-290 -> lines[0:290]
# 2. 291-423 -> lines[290:423]

detailed_ranges = [
    ("01-DataStructures_Algorithms", 1, 290),
    ("02-DesignPatterns", 291, 423),
    ("03-CSharp_DotNet", 424, 694),
    ("04-Architecture_SystemDesign", 695, 956),
    ("05-Databases", 957, 1537),
    ("06-DevOps_Messaging", 1538, -1)
]

cheatsheet_ranges = [
    ("01-DataStructures_Algorithms", 1, 151),
    ("02-DesignPatterns", 152, 248),
    ("03-CSharp_DotNet", 249, 428),
    ("04-Architecture_SystemDesign", 429, 712),
    ("05-Databases", 713, 1405),
    ("06-DevOps_Messaging", 1406, -1)
]

split_file("notes/notes.md", detailed_ranges, "Detailed")
split_file("notes/notes-titles.md", cheatsheet_ranges, "Cheatsheet")
