# Gran Turismo 5 Car hack workshop - Save Editor

A Windows desktop application for editing game save files for Gran Turismo 5.

This tool provides a simple graphical interface to modify various aspects of your save data, including equipped car parts and other vehicle attributes.

## Downloads

To download the latest build for Windows, please visit the **[Releases](https://github.com/HeylonNHP/GT5-Car-hack-workshop/releases)** page of this repository.

## Running on Linux (via Wine)

This application is built with Windows Forms and targets `net10.0-windows`, so it cannot run natively on Linux. However, you can run it on Linux using Wine (or Proton). Two options are provided below:

### Quick start with helper script

Prerequisites:
- .NET SDK 10 or newer installed (`dotnet --info`)
- Wine installed (`wine --version`)

Steps:
1. Make the script executable: `chmod +x scripts/run-on-linux-with-wine.sh`
2. Run it: `./scripts/run-on-linux-with-wine.sh`

By default, this publishes a 32‑bit Windows build (more compatible under Wine) and launches it. Options:
- `--x64` publish as 64‑bit
- `--self-contained` publish with the .NET runtime included
- `--single-file` publish as a single executable (implies self‑contained)
- `--clean` cleans previous build outputs first

Examples:
- 64‑bit framework‑dependent: `./scripts/run-on-linux-with-wine.sh --x64`
- 32‑bit self‑contained single file: `./scripts/run-on-linux-with-wine.sh --single-file`

### Manual steps

1) Install prerequisites
- .NET SDK: https://learn.microsoft.com/dotnet/core/install/linux
- Wine: use your distro package manager (e.g., `sudo apt install wine` or `sudo dnf install wine`)

2) Publish a Windows build on Linux

You can build the Windows target from Linux (cross‑compile):

```
dotnet publish "GT5 Car hack workshop/GT5 Car hack workshop.csproj" \
  -c Release -r win-x86 --self-contained false
```

This produces output under:
`GT5 Car hack workshop/bin/Release/net10.0-windows/win-x86/publish/`

Optional variations:
- 64‑bit: replace `win-x86` with `win-x64`
- Self‑contained: add `--self-contained true`
- Single file: add `-p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true --self-contained true`

3) Run with Wine

```
wine "GT5 Car hack workshop/bin/Release/net10.0-windows/win-x86/publish/GT5 Car hack workshop.exe"
```

### Notes and troubleshooting
- If UI fonts look off, install core fonts in Wine (e.g., `winetricks corefonts`), or enable font smoothing in Wine.
- If you encounter issues with 64‑bit builds under Wine, prefer the 32‑bit target (`win-x86`).
- Proton (Steam) can also run the .exe; place the published folder somewhere accessible and create a non‑Steam game pointing to the exe, enabling Proton.
- Native Linux support would require porting the UI to a cross‑platform framework (e.g., Avalonia). The current WinForms app will continue to require Wine on Linux.