#!/usr/bin/env python3
"""Add the game's tyre grades to the parts catalogue (partscatalogue.db).

The editor's two tyre fields are game tables 51 (front tyres) and 52
(rear tyres). A save stores a tyre as an 8-byte key whose LAST byte is a gear
slot (``00 00 00 27 00 33 00 <slot>`` for the front axle, ``00 00 00 27 00 34 00
<slot>`` for the rear). A slot is a grade, not a part id, and every grade fits
every car, so the game's own part tables carry no rows for these fields. This
script fills that gap in the generated catalogue so the two tyre categories look
exactly like every other category at runtime.

What it writes, for each of categories 51 and 52:

* 15 rows in ``Parts``  with ``PartKey = Level = slot`` and ``CarId = 0``
  (0 = "generic"/universal, the same convention the catalogue already uses for
  the 230 car-less rows it ships with);
* 15 rows in ``Items``  with ``(Category, Level = slot, Name)``.

The slot -> grade map is the game's 15 purchasable grades and is kept here so
the script is self-contained and reproducible.

The script is idempotent: it deletes whatever is already there for 51 and 52 and
re-inserts the canonical set, so running it any number of times leaves the same
logical rows. It only ever touches categories 51 and 52; every other table and
row is left alone. It finishes with ``PRAGMA optimize`` and ``VACUUM``.

Usage (from the repository root):

    python3 tools/generate_tyres.py                 # edits the shipped catalogue
    python3 tools/generate_tyres.py path/to/other.db

Requires only the Python standard library.
"""

from __future__ import annotations

import os
import sqlite3
import sys

# Game table / catalogue category ids for the editor's two tyre fields.
FRONT_TYRES = 51
REAR_TYRES = 52
TYRE_CATEGORIES = (FRONT_TYRES, REAR_TYRES)

# The 15 real, purchasable tyre grades, in slot order. This IS the catalogue for
# a tyre field: the slot in the save indexes straight into this list.
TYRE_GRADES = {
    0: "Comfort Hard",
    1: "Comfort Medium",
    2: "Comfort Soft",
    3: "Sports Hard",
    4: "Sports Medium",
    5: "Sports Soft",
    6: "Sports Super Soft",
    7: "Racing Hard",
    8: "Racing Medium",
    9: "Racing Soft",
    10: "Racing Super Soft",
    11: "Racing Intermediate",
    12: "Racing Rain",
    13: "Dirt",
    14: "Snow",
}

# Default catalogue, relative to the repository root (this file lives in tools/).
_DEFAULT_RELATIVE = os.path.join("GT5 Car hack workshop", "partscatalogue.db")


def default_db_path() -> str:
    root = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
    return os.path.join(root, _DEFAULT_RELATIVE)


def regenerate(db_path: str) -> None:
    if not os.path.isfile(db_path):
        raise SystemExit(f"catalogue not found: {db_path}")

    slots = sorted(TYRE_GRADES)

    connection = sqlite3.connect(db_path)
    try:
        # CarId 0 is not a real car (the Cars table starts at id 21): it is the
        # catalogue's own "no car / universal" marker, and the shipped file
        # already has car-less rows. Keep FK enforcement out of the way so the
        # marker is accepted whatever the caller's sqlite build defaults to.
        connection.execute("PRAGMA foreign_keys = OFF")

        for category in TYRE_CATEGORIES:
            connection.execute("DELETE FROM Parts WHERE Category = ?", (category,))
            connection.execute("DELETE FROM Items WHERE Category = ?", (category,))

        # Parts.Id is INTEGER PRIMARY KEY (autoincrement). The tyre rows are the
        # highest ids in the file, so deleting them first frees the same range and
        # a re-run reuses the same ids - the result is stable across runs.
        for category in TYRE_CATEGORIES:
            for slot in slots:
                connection.execute(
                    "INSERT INTO Parts (Category, PartKey, Level, CarId) "
                    "VALUES (?, ?, ?, 0)",
                    (category, slot, slot),
                )
                connection.execute(
                    "INSERT INTO Items (Category, Level, Name) VALUES (?, ?, ?)",
                    (category, slot, TYRE_GRADES[slot]),
                )

        connection.commit()

        connection.execute("PRAGMA optimize")
        connection.commit()
        connection.execute("VACUUM")
        connection.commit()
    finally:
        connection.close()


def report(db_path: str) -> None:
    connection = sqlite3.connect(f"file:{db_path}?mode=ro", uri=True)
    try:
        print(f"catalogue: {db_path}")

        for category in TYRE_CATEGORIES:
            label = "front tyres" if category == FRONT_TYRES else "rear tyres"
            parts = connection.execute(
                "SELECT COUNT(*) FROM Parts WHERE Category = ?", (category,)
            ).fetchone()[0]
            items = connection.execute(
                "SELECT COUNT(*) FROM Items WHERE Category = ?", (category,)
            ).fetchone()[0]
            print(f"  category {category} ({label}): "
                  f"Parts={parts}, Items={items}")

        print("  sample Parts rows (category, partkey, level, carid):")
        for row in connection.execute(
            "SELECT Category, PartKey, Level, CarId FROM Parts "
            "WHERE Category IN (51, 52) ORDER BY Category, Level LIMIT 5"
        ):
            print(f"    {row}")

        print("  sample Items rows (category, level, name):")
        for row in connection.execute(
            "SELECT Category, Level, Name FROM Items "
            "WHERE Category IN (51, 52) ORDER BY Category, Level LIMIT 5"
        ):
            print(f"    {row}")

        total_parts = connection.execute("SELECT COUNT(*) FROM Parts").fetchone()[0]
        total_items = connection.execute("SELECT COUNT(*) FROM Items").fetchone()[0]
        print(f"  totals now: Parts={total_parts}, Items={total_items}")

        integrity = connection.execute("PRAGMA integrity_check").fetchone()[0]
        print(f"  integrity_check: {integrity}")
    finally:
        connection.close()


def main(argv: list[str]) -> int:
    db_path = os.path.abspath(argv[1]) if len(argv) > 1 else default_db_path()
    regenerate(db_path)
    report(db_path)
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
