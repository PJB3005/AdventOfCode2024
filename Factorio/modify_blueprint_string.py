from typing import Any, Iterable
import json
import zlib
from base64 import b64decode, b64encode

INPUT_STRING = "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"

def load_bp_string(bp: str) -> Any:
    d = b64decode(bp[1:])
    raw = zlib.decompress(d)
    return json.loads(raw.decode())

def dump_bp_string(data: Any) -> bytes:
    raw = json.dumps(data).encode()
    d = zlib.compress(raw, level=9)
    return b"0" + b64encode(d)


def parse_inputs() -> tuple[list[int], list[int]]:
    a = []
    b = []
    with open("input.txt", "r") as f:
        for line in f.readlines():
            row = line.strip().split()
            a.append(int(row[0]))
            b.append(int(row[1]))

    return (a, b)

blueprint = load_bp_string(INPUT_STRING)
list_a, list_b = parse_inputs()

def find_combinators(bp: Any) -> Iterable[Any]:
    for entity in bp["blueprint"]["entities"]:
        if entity["name"] != "constant-combinator":
            continue

        yield entity

def load_signal_list():
    return json.load(open("signals.json", "r"))

def multiply_signals(signals):
    def apply_quality(signal: dict, quality):
        signal = signal.copy()
        signal["quality"] = quality
        return signal
    
    new_signals = []
    for s in signals:
        new_signals.append(apply_quality(s, "normal"))
        new_signals.append(apply_quality(s, "uncommon"))
        new_signals.append(apply_quality(s, "rare"))
        new_signals.append(apply_quality(s, "epic"))
        new_signals.append(apply_quality(s, "legendary"))

    return new_signals

signals = multiply_signals(load_signal_list())
combinator_a, combinator_b = find_combinators(blueprint)

def apply_list(combinator: Any, values: list[int]):
    filters = []
    i = 0
    for value in values:
        filter = signals[i].copy()

        filter["index"] = i + 1
        filter["comparator"] = "="
        filter["count"] = value

        filters.append(filter)
        i += 1

    combinator["control_behavior"]["sections"]["sections"][0]["filters"] = filters

apply_list(combinator_a, list_a)
apply_list(combinator_b, list_b)

open("blueprint.out", "wb").write(dump_bp_string(blueprint))
open("blueprint.out.json", "w").write(json.dumps(blueprint))

print("Blueprint string is in blueprint.out")
