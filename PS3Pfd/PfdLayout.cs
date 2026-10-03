namespace PS3Pfd;

internal static class PfdLayout
{
    internal const string ContainerFileName = "PARAM.PFD";
    internal const string ParamSfoFileName = "PARAM.SFO";

    internal const ulong Magic = 0x50464442;
    internal const ulong VersionV3 = 3;
    internal const ulong VersionV4 = 4;

    internal const int ContainerSize = 0x8000;
    internal const int BlockSize = 16;
    internal const int HashSize = 20;
    internal const int HashKeySize = 20;
    internal const int KeySize = 16;
    internal const int SecureFileIdSize = 16;
    internal const int EntryNameSize = 65;
    internal const long MaxFileSize = 1L << 30;

    internal const int HeaderKeyOffset = 16;
    internal const int HeaderKeySize = 16;
    internal const int SignatureOffset = 32;
    internal const int SignatureSize = 64;
    internal const int BottomHashOffset = SignatureOffset;
    internal const int TopHashOffset = SignatureOffset + HashSize;
    internal const int HashKeyOffset = SignatureOffset + 2 * HashSize;

    internal const int HashTableOffset = 96;
    internal const int HashTableHeaderSize = 24;
    internal const int FirstEntryOffset = HashTableOffset + HashTableHeaderSize;
    internal const int EntryIndexSize = 8;

    internal const int EntrySize = 272;
    internal const int EntryNameOffset = 8;
    internal const int EntryKeyOffset = 80;
    internal const int EntryKeySize = 64;
    internal const int EntryFileHashesOffset = 144;
    internal const int EntryFileHashCount = 4;
    internal const int EntryFileSizeOffset = 264;
    internal const int EntryHashedDataSize = 192;
}
