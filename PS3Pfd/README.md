# PS3Pfd

Decrypts and re-encrypts PS3 savedata, in process, with no external tool.

A PS3 save folder holds a `PARAM.PFD` parameter file alongside the game's data.
The data file is stored encrypted, the parameter file records what is in the
folder and carries the hashes that prove it, and a console will reject a save
whose hashes do not add up. PS3Pfd reads and rewrites that container, and
transforms the files it lists, so a save can be opened, edited and written back.

It is a plain class library. It does not know anything about the game whose save
it is handling beyond that game's key.

## Two kinds of savedata

Emulator saves and console saves need different things, and the difference is
whether a parameter file exists at all. Emulators write the data file as
plaintext and no `PARAM.PFD`, so there is nothing to decrypt and nothing to
verify.

| | Container present | Container absent |
|---|---|---|
| `HasContainer` | `true` | `false` |
| `DetectBodies` | `Encrypted` or `Plaintext` | `Plaintext` |
| `Decrypt` | decrypts every listed file, rebuilds the hashes | nothing, returns untouched |
| `Encrypt` | encrypts every listed file, rebuilds the hashes | nothing, returns untouched |
| `Validate` | checks the container and the files | reports that there is nothing to check |

Both cases are normal. An emulator folder is not a failure and raises nothing.

## Using it

```csharp
var savedata = SaveCrypto.SavedataDirectoryOf(saveFilePath);

SaveCrypto.Decrypt(savedata);
var save = File.ReadAllBytes(saveFilePath);

// edit, then

File.WriteAllBytes(saveFilePath, save);
SaveCrypto.Encrypt(savedata);
```

`Decrypt` and `Encrypt` are idempotent and safe to call when there is nothing to
do. Decrypting twice does not decrypt plaintext a second time, and encrypting an
already-encrypted folder does nothing.

Every method takes the **savedata folder**, not the file, because the container
is what spans the folder: `PARAM.PFD`, the game's data file, and any icons.

## API

| Member | Returns | Notes |
|---|---|---|
| `SaveCrypto.HasContainer(directory)` | `bool` | whether a parameter file is present. Says nothing about it being valid |
| `SaveCrypto.DetectBodies(directory)` | `SaveBodies` | whether the listed files currently look encrypted |
| `SaveCrypto.Decrypt(directory, keys?)` | `SaveOperation` | in place |
| `SaveCrypto.Encrypt(directory, keys?)` | `SaveOperation` | in place |
| `SaveCrypto.Validate(directory, keys?)` | `PfdValidation` | reports, never throws |
| `SaveCrypto.SavedataDirectoryOf(filePath)` | `string` | the folder a save file lives in |
| `SaveKeys.Gt5` | `SaveKeys` | the key for `BCES00569`; pass another for a different title |

`keys` defaults to `SaveKeys.Gt5`, so the whole surface can be called with just a
directory.

`SaveOperation` reports `Format`, the resulting `Bodies`, and `FilesTransformed`
with a `ChangedAnything` convenience. `SaveFormat` is `EmulatorPlaintext` or
`Ps3Container`; `SaveBodies` is `Plaintext` or `Encrypted`.

`PfdValidation` carries four nullable hash flags, a `Problem` string, and
`IsValid`. `null` means *not applicable* rather than *failed* — an emulator
folder has no hashes to check, so its flags come back `null` and `IsValid` is
`true`. When something is wrong, `Problem` says what and `IsValid` is `false`:

```
top=True bottom=True entries=True files=True                          a healthy console save
top= bottom= entries= files=                                          an emulator folder
valid=False files=False problem=GT5.0 is listed in the parameter file
                               but is missing from the savedata folder
```

Anything that cannot be acted on — an unreadable container, a missing file, a
folder that is not a save — raises `SaveCryptoException` naming what failed.
`Validate` is the exception: it exists to describe broken states, so it reports
them instead.

## What it touches, and what it does not

- **Transforms** only the files the container lists, and only the ones it is
  asked to. A file sitting in the folder that the container does not list is
  never touched.
- **Never transforms `PARAM.SFO`.** Four of its hash slots are keyed by
  console-specific values that cannot be recomputed without the console, so
  changing the file could not be made valid. It still participates in the
  container's own hashes, so the container stays correct.
- **Does not implement trophy keys.** Only the per-game secure file id.
- Rewrites the container atomically, through a temporary file, so an interrupted
  write cannot leave a half-written parameter file. Files themselves are written
  in place, so back the folder up before enabling this on a save you care about.

## How it works

A `PARAM.PFD` is a fixed 32 KiB, big-endian structure: a header, an encrypted
signature block, a hash bucket table, a table of 272-byte entries, and a table
of per-bucket hashes.

The buffer **is** the model. The container parses into small views over the
original bytes and only ever patches the ranges it computes, so entry padding,
reserved-but-unused entries and the `PARAM.SFO` entry survive a round trip
untouched. Rebuilding the file from an object graph would quietly lose them.

Three cryptographic steps, all AES-128 and HMAC-SHA1:

1. The signature block decrypts under a public constant key to reveal the
   container's own hash key — so a container can be validated offline, with no
   console.
2. Each entry's sealed key unwraps under that same constant, giving the file's
   cipher key. That key comes from the title's **secure file id**, which is why
   a game only needs to supply 16 bytes.
3. File bodies use a per-block construction: each 16-byte block is combined with
   an AES-ECB keystream derived from its own block index.

Hashes are then recomputed honestly rather than zeroed or faked, because every
key involved is either public or stored inside the save.

## Correctness

The library is verified by a 40-check harness covering: byte-exact container
round trips with slack and padding preserved, file round trips returning the
original bytes exactly, both container versions, idempotent decrypt and encrypt,
the emulator pass-through leaving files byte-identical, and every error path.
The format and the cipher were additionally implemented a second time from
scratch, independently, and both produce byte-identical output for the same key.

**Not proven:** interoperability with real console hardware. There is no
encrypted console save and no console available to the development machine, so
every encrypted test vector is machine-generated. A genuine `PARAM.PFD` would
settle most of it — its signature block decrypts under a public key, so the
parser, the hash rebuild and the bucket walk are all checkable offline — but the
per-title file-body key can only be confirmed against a save from a console.

Also unknown: which container version this title ships (both work), and whether
a real file ever uses the empty-bucket boundary that the format's own tools
leave ambiguous.

## Building

```
dotnet build PS3Pfd.csproj -c Debug
```

Requires .NET 10. No packages, no P/Invoke, no platform-specific code — it builds
and runs the same on Linux and Windows. Cryptography comes from
`System.Security.Cryptography` only.

The library contains no comments. Names and structure carry the meaning; if a
change seems to need a comment, change the shape of the code instead.
