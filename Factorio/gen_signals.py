#!/usr/bin/env python3

import json

raw_data_f = open("data-raw-dump.json", "r")
raw_data = json.load(raw_data_f)

output = []

for id, data in raw_data["item"].items():
    if data.get("parameter", False) or data.get("hidden", False):
        continue

    output.append({"type": "item", "name": id})

print(len(output))

json.dump(output, open("signals.json", "w"))
