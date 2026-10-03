using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace PS3Pfd;

internal readonly struct PfdEntry
{
    private readonly byte[] container;
    private readonly int start;

    internal PfdEntry(byte[] container, int start)
    {
        this.container = container;
        this.start = start;
    }

    internal ulong AdditionalIndex => BinaryPrimitives.ReadUInt64BigEndian(container.AsSpan(start, PfdLayout.EntryIndexSize));

    internal string Name => ReadName();

    internal bool IsParamSfo => Name.Equals(PfdLayout.ParamSfoFileName, StringComparison.OrdinalIgnoreCase);

    internal long FileSize => (long)BinaryPrimitives.ReadUInt64BigEndian(container.AsSpan(start + PfdLayout.EntryFileSizeOffset, 8));

    internal void SetFileSize(long fileSize) =>
        BinaryPrimitives.WriteUInt64BigEndian(container.AsSpan(start + PfdLayout.EntryFileSizeOffset, 8), (ulong)fileSize);

    internal ReadOnlySpan<byte> SealedKey => container.AsSpan(start + PfdLayout.EntryKeyOffset, PfdLayout.EntryKeySize);

    internal ReadOnlySpan<byte> FileHash(int index) =>
        container.AsSpan(start + PfdLayout.EntryFileHashesOffset + index * PfdLayout.HashSize, PfdLayout.HashSize);

    internal void SetFileHash(int index, ReadOnlySpan<byte> hash) =>
        hash.CopyTo(container.AsSpan(start + PfdLayout.EntryFileHashesOffset + index * PfdLayout.HashSize, PfdLayout.HashSize));

    internal void AppendHashInput(IncrementalHash hmac)
    {
        hmac.AppendData(container, start + PfdLayout.EntryNameOffset, PfdLayout.EntryNameSize);
        hmac.AppendData(container, start + PfdLayout.EntryKeyOffset, PfdLayout.EntryHashedDataSize);
    }

    private string ReadName()
    {
        var raw = container.AsSpan(start + PfdLayout.EntryNameOffset, PfdLayout.EntryNameSize);
        var terminator = raw.IndexOf((byte)0);
        return Encoding.ASCII.GetString(raw[..(terminator < 0 ? PfdLayout.EntryNameSize : terminator)]);
    }
}
