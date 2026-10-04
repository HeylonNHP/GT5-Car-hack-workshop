#!/usr/bin/env python3
"""
One-shot, IDEMPOTENT patcher: writes the game's real per-part type into
partscatalogue.db.Parts.PartType.

The type is the 'category' byte each SpecDB part table carries per row (verified in a
previous pass against GT5_JP3010's PARTS_* enums; the distribution checksums below are the
identity proof - ANY mismatch aborts BEFORE any write, protecting against a different
SpecDB revision). With PartType present, PartInstaller keys the game's ownership mask
(PurchaseBitBase + type) from the part's true type instead of its stock-relative catalogue
Level. Rows whose key cannot be resolved in SpecDB stay NULL; NULL is never rewritten with 0
(0 is a legitimate type, and the Level-fallback families - brake controller, displacement,
intercooler - must stay NULL so their Level rule keeps working).

Idempotent: running again on a patched database writes 0 rows (only rows whose stored value
differs from the proven type are updated). The only structural change is adding a nullable
integer PartType column to Parts; no index, no constraint, no other table or column is ever
touched, and the database ends with one VACUUM.

Usage:
    python3 tools/patch_part_types.py <path-to-partscatalogue.db> [--specdb DIR] [--force]

Exit codes: 0 ok, 2 SpecDB table/shape/checksum mismatch, 3 catalogue row-count guard.
"""

import argparse
import importlib.util
import os
import sqlite3
import sys
from collections import Counter

DEFAULT_SPECDB = "/mnt/Secondary/GT5_update_extract/specdb/GT5_JP3010"
DEFAULT_DBT_SRC = "/home/heylon/GT5/tools/scripts"
EXPECTED_PARTS_ROWS = 116_472

# catId -> (table name, row size, category-byte offset in the row record, required value
# distribution {type byte: row count}). The dict must hold EXACTLY - it is the guard
# against patching from a SpecDB revision this table set does not describe.
TYPED = {
    2:  ("BRAKE",            7,   2, {0: 1147, 1: 695}),
    4:  ("SUSPENSION",       147, 26, {0: 832, 1: 849, 2: 890, 4: 1138}),          # no value 3 or 5 exists
    9:  ("LIGHTWEIGHT",      6,   4, {1: 830, 2: 855, 3: 864, 4: 888, 5: 901}),  # note said {2: 854}; JP3010 decodes to 855 (one more type-2 row)
    11: ("DRIVETRAIN",       21,  14, {0: 1111, 1: 234}),
    12: ("GEAR",             45,  34, {0: 874, 1: 549, 2: 572, 3: 1134}),
    14: ("NATUNE",           14,  10, {0: 1, 1: 857, 2: 881, 3: 996, 4: 1, 5: 1}),  # progressive: stage == type; the note's figure {1:1147,2:1148,3:1149} (3447 rows) mismatches the JP3010 file, which is 1:1 with the catalogue at 2737
    15: ("TURBINEKIT",       21,  10, {0: 386, 1: 643, 2: 769, 3: 771, 4: 230, 5: 180}),
    17: ("COMPUTER",         12,  8,  {1: 860}),
    19: ("MUFFLER",          12,  10, {0: 293, 1: 855, 2: 858, 3: 846}),
    20: ("CLUTCH",           8,   2,  {0: 2, 1: 812, 2: 860, 3: 855}),
    21: ("FLYWHEEL",         7,   2,  {1: 812, 2: 860, 3: 855}),
    22: ("PROPELLERSHAFT",   8,   2,  {1: 545}),
    23: ("LSD",              27,  2,  {0: 907, 1: 1136, 2: 14}),
    27: ("SUPERCHARGER",     9,   8,  {1: 197}),
    28: ("INTAKE_MANIFOLD",  10,  6,  {1: 892}),
    29: ("EXHAUST_MANIFOLD", 7,   6,  {1: 892}),
    30: ("CATALYST",         10,  6,  {1: 859, 2: 886}),
    31: ("AIR_CLEANER",      7,   6,  {1: 833, 2: 875}),
    # The two bit-less families whose SpecDB tables DO exist. Their type byte is constant 0
    # (ENGINE: bit 0 is the sentinel that must never be set; CHASSIS: no purchase bit exists),
    # so these rows carry PartType 0 - a legitimate type - and produce no bit in the installer,
    # exactly as a NULL PartType would; asserting their decoded checksums keeps the revision
    # guard complete. The byte is 0 at several offsets in both tables and the checksum
    # {0: rowcount} holds for each candidate column, so the conclusion ("family type constant 0")
    # is independent of which all-0 column the offset names.
    13: ("ENGINE",           116, 0,  {0: 1150}),   # type byte 0 also at +1/+4/+5/+8/+9/+12/+13/+16/+17/+18/+20/+21/+24/+25/+32/+82/+113/+115
    7:  ("CHASSIS",          68,  30, {0: 1149}),   # type byte 0 also at +31/+32/+37/+61/+62/+64/+67
}

# Families that stay NULL in this patch: NOS(26), ASCC(5), TCSC(6), BRAKECONTROLLER(3),
# DISPLACEMENT(16), INTERCOOLER(18) and BONNET(35) (no .dbt in JP3010 - the bit families among
# them keep their Level rule at runtime), RACINGMODIFY(8) (255 = n/a), STEER(10) (values 25/47
# are not a type), and the tyre tables (their bytes are purchase-position codes, and tyres are
# refused at install anyway).

# The research note's reference parts: catId -> part key. Their .idi labels and decoded type
# bytes are printed as identity evidence; the harness proves they land on bits
# 61 / 12 / 33 / 64 / 68 / 176 / 179.
REFERENCE_PARTS = {
    19: 0x0F7D,  # muffler     -> bit 61
    4:  0x188C,  # suspension  -> bit 12
    12: 0x1657,  # gear        -> bit 33
    20: 0x0FB7,  # clutch      -> bit 64
    21: 0x0A75,  # flywheel    -> bit 68
    30: 0x02A6,  # catalyst    -> bit 176
    31: 0x0586,  # air cleaner -> bit 179
}


def load_dbt_module(folder):
    path = os.path.join(folder, "dbt.py")
    if not os.path.exists(path):
        print(f"ABORT: no dbt.py under {folder}", file=sys.stderr)
        raise SystemExit(2)
    spec = importlib.util.spec_from_file_location("gt5_dbt_for_patch", path)
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)
    return module


def decode_specdb_types(specdb_dir, dbt_module):
    """Decode every typed table ONCE into {(catId, partKey): category byte}.
    Asserts every table's shape and value distribution; raises SystemExit on any mismatch -
    BEFORE any database write happens."""
    part_types = {}
    evidence = {}

    for cat_id, (name, rowsize, offset, required) in sorted(TYPED.items()):
        path = os.path.join(specdb_dir, name + ".dbt")
        if not os.path.exists(path):
            print(f"ABORT: missing SpecDB table {path}", file=sys.stderr)
            raise SystemExit(2)

        table = dbt_module.DBT(path)
        got = Counter()
        for i in range(table.rowcount):
            row_id, data = table.get_row(i)
            if len(data) != rowsize or offset >= len(data):
                print(f"ABORT: {name} row {i} (key {row_id}) has {len(data)} bytes, "
                      f"expected rowsize {rowsize} - shape mismatch", file=sys.stderr)
                raise SystemExit(2)
            got[data[offset]] += 1
            key = (cat_id, row_id)
            if key in part_types and part_types[key] != data[offset]:
                print(f"ABORT: {name} duplicate key 0x{row_id:X} with conflicting type bytes",
                      file=sys.stderr)
                raise SystemExit(2)
            part_types[key] = data[offset]

        if dict(got) != required:
            print(f"ABORT: {name} (catId {cat_id}, offset +{offset}) value distribution "
                  f"does not match (wrong SpecDB revision?)", file=sys.stderr)
            print(f"  required: {dict(sorted(required.items()))}", file=sys.stderr)
            print(f"  actual:   {dict(sorted(got.items()))}", file=sys.stderr)
            raise SystemExit(2)

        labels = {}
        idi_path = os.path.join(specdb_dir, name + ".idi")
        if os.path.exists(idi_path):
            labels = dbt_module.parse_idi(idi_path)
        samples = []
        if cat_id in REFERENCE_PARTS:
            ref_key = REFERENCE_PARTS[cat_id]
            samples.append(f"ref 0x{ref_key:X} -> 0x{ref_key:X} type "
                           f"{part_types[(cat_id, ref_key)]} label '{labels.get(ref_key, '?')}'")
        if labels:
            shown = 0
            for row_id, label in sorted(labels.items()):
                if shown >= 2 or (cat_id, row_id) not in part_types:
                    continue
                samples.append(f"0x{row_id:X} '{label}'")
                shown += 1

        evidence[cat_id] = (table.rowcount, got, samples)

    return part_types, evidence


def main():
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("database", help="path to partscatalogue.db")
    ap.add_argument("--specdb", default=DEFAULT_SPECDB, help="SpecDB table folder")
    ap.add_argument("--dbt-src", default=DEFAULT_DBT_SRC, help="folder holding dbt.py")
    ap.add_argument("--force", action="store_true", help="skip the 116 472-row guard")
    args = ap.parse_args()

    path = args.database
    if not os.path.exists(path):
        print(f"ABORT: no catalogue file at {path}", file=sys.stderr)
        raise SystemExit(3)

    conn = sqlite3.connect(path)
    conn.isolation_level = None  # explicit transactions below; VACUUM works in autocommit
    try:
        # (a) GUARD: refuse to run against a catalogue of a different shape.
        rows_total = conn.execute("SELECT COUNT(*) FROM Parts").fetchone()[0]
        if rows_total != EXPECTED_PARTS_ROWS and not args.force:
            print(f"ABORT: Parts has {rows_total} rows, expected {EXPECTED_PARTS_ROWS}. "
                  f"Refusing to run without --force.", file=sys.stderr)
            raise SystemExit(3)

        # (b) SCHEMA: add the nullable PartType column if it is missing (checked again later,
        #     inside the transaction, so two instances never race).
        cols = [row[1] for row in conn.execute("PRAGMA table_info(Parts)")]
        has_column = "PartType" in cols
        size_before = os.path.getsize(path)

        # (c) DECODE + ASSERT: every checksum must hold before a single byte of the catalogue
        #     is written.
        part_types, evidence = decode_specdb_types(args.specdb, load_dbt_module(args.dbt_src))
        for cat_id, (table_rows, got, samples) in evidence.items():
            print(f"{TYPED[cat_id][0]} (catId {cat_id}): specdb rows {table_rows}, "
                  f"offset +{TYPED[cat_id][2]}, checksum OK {dict(sorted(got.items()))}")
            for sample in samples:
                print(f"    {sample}")

        # (d) WRITE PASS: one transaction; only (Category, PartKey) keys that decode resolve,
        #     and only rows whose stored value differs are updated (idempotence).
        conn.execute("BEGIN IMMEDIATE")
        cols_now = [row[1] for row in conn.execute("PRAGMA table_info(Parts)")]
        if "PartType" not in cols_now:
            conn.execute('ALTER TABLE Parts ADD COLUMN "PartType" INTEGER')

        cur = conn.execute("SELECT Category, PartKey, PartType FROM Parts")
        plan = []
        typed = 0
        already = 0
        written_per_family = Counter()
        for category, part_key, current in cur:
            target = part_types.get((category, part_key))
            if target is None:
                continue
            typed += 1
            if current != target:
                plan.append((target, category, part_key))
                written_per_family[category] += 1
            else:
                already += 1
        conn.executemany("UPDATE Parts SET PartType = ? WHERE Category = ? AND PartKey = ?", plan)
        conn.commit()

        # (e) one VACUUM, then the report.
        conn.execute("VACUUM")
        size_after = os.path.getsize(path)

        typed_now, null_now = conn.execute(
            "SELECT SUM(PartType IS NOT NULL), SUM(PartType IS NULL) FROM Parts").fetchone()
        integrity = conn.execute("PRAGMA integrity_check").fetchone()[0]
        print()
        print(f"Parts rows: {rows_total}; PartType set: {typed_now} (this run: "
              f"{typed - already} changed, {already} already correct); unresolvable NULL kept: {null_now}")
        print(f"DB size: {size_before} -> {size_after} bytes ({size_after - size_before:+d})")
        print(f"integrity_check: {integrity}")
        for cat_id, (name, _, _, required) in sorted(TYPED.items()):
            cat_rows, cat_typed = conn.execute(
                "SELECT COUNT(*), SUM(PartType IS NOT NULL) FROM Parts WHERE Category = ?",
                (cat_id,)).fetchone()
            written = written_per_family.get(cat_id, 0)
            print(f"  {name:16s} catId {cat_id:2d}: catalogue rows {cat_rows}, typed "
                  f"{cat_typed}, newly written this run {written}, untyped-left-NULL "
                  f"{cat_rows - cat_typed}")
        if integrity != "ok":
            raise SystemExit(2)
    finally:
        conn.close()


if __name__ == "__main__":
    main()