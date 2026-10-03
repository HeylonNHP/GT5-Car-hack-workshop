# Gran Turismo 5 Car Hack Workshop

A save editor for **Gran Turismo 5 (2.14)**.

Open a save, change what you want, write it back. Works with saves copied from a
real PS3 and with RPCS3, which stores saves unencrypted.

## Download

Grab the latest **[Release](https://github.com/HeylonNHP/GT5-Car-hack-workshop/releases)**
for Windows. Unzip it and run `GT5 Car hack workshop.exe` — it is self-contained,
so there is nothing to install first.

## What you can change

- **Parts** — drop-downs for every part category, each entry showing which car it
  came from, so you can fit any car's part to any other car. Plus a list of known
  tunes that fill in a whole setup at once.
- **Paint chips** — add and remove chips, and browse all 3,400 colours with
  search, finish filtering and sorting.
- **Credits, odometer, horsepower multiplier, grip.**
- **Hacks** — remove the spoiler, hood or bumpers; make the car 4WD with an
  adjustable torque split; set bad or good oil; the borrow glitch; downforce,
  aero and ride height; a transmission editor; and a custom performance editor.
- **Save only**, or **Save and encrypt**.

## Using it

1. Point the app at your save file and press **Load data**.
2. Change whatever you like.
3. Press **Save and encrypt** to write it back.

**Back up your save first.** The app makes a timestamped copy in its `Backups`
folder every time you load, but it is not a substitute for doing it yourself.

Where the save lives:

- **RPCS3** — `~/.config/rpcs3/dev_hdd0/home/<user>/savedata/BCES00569-GAME/GT5.0`
- **Real PS3** — copy the `BCES00569-GAME` folder off the console with a
  file manager or FTP, then point the app at the `GT5.0` inside it.

## Requirements

- Gran Turismo 5 version 2.14 (title id `BCES00569`).
- **Windows**: nothing beyond the release download.
- **Linux**: runs natively — this is an Avalonia app, not a Windows-only one, so
  Wine is not needed. Build it from source with the .NET 10 SDK.

No extra tools are required. Save decryption and encryption are built in.

## Building from source

```bash
dotnet build "GT5 Car hack workshop.sln" -c Debug

# a Windows build
dotnet publish "GT5 Car hack workshop/GT5 Car hack workshop.csproj" \
  -c Release -r win-x64 --self-contained true
```

Everything targets `net10.0` and builds on Linux, Windows or macOS. Pushing to
`master` builds and publishes a Windows release automatically.

## What's in the repository

| | |
|---|---|
| `GT5 Car hack workshop/` | the app — Avalonia, .NET 10, cross-platform |
| `PS3Pfd/` | save decryption and encryption, in process |
| `PS3Pfd/README.md` | how the save format works, and what is verified |
| `partscatalogue.db` | parts and tunes, generated from the game's own files |
| `partsdatabase.db` | the app's parts data |
| `Backups/`, next to the app | timestamped copies made on load |

## Caveats

Editing a save is at your own risk, and changes that the game considers invalid
can corrupt a save or a car. Keep the backups.

The save crypto is well tested but has never been verified against a save written
by real console hardware — see `PS3Pfd/README.md` for exactly what has and has
not been proven.
