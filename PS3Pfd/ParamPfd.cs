using System.Buffers.Binary;
using System.Security.Cryptography;

namespace PS3Pfd;

internal sealed class ParamPfd
{
    private readonly byte[] container;
    private readonly byte[] realHashKey;
    private readonly ulong capacity;
    private readonly ulong numReserved;
    private readonly ulong numUsed;
    private readonly int entryTableStart;
    private readonly int signatureTableStart;

    private ParamPfd(byte[] container, ulong capacity, ulong numReserved, ulong numUsed, int entryTableStart, byte[] realHashKey)
    {
        this.container = container;
        this.capacity = capacity;
        this.numReserved = numReserved;
        this.numUsed = numUsed;
        this.entryTableStart = entryTableStart;
        this.realHashKey = realHashKey;
        signatureTableStart = entryTableStart + (int)numReserved * PfdLayout.EntrySize;
    }

    internal static ParamPfd Parse(byte[] container)
    {
        if (container.Length < PfdLayout.ContainerSize)
            throw new SaveCryptoException($"The parameter file is {container.Length} bytes, but a PS3 parameter file is {PfdLayout.ContainerSize} bytes.");

        if (BinaryPrimitives.ReadUInt64BigEndian(container) != PfdLayout.Magic)
            throw new SaveCryptoException("The parameter file does not start with the expected signature.");

        var version = BinaryPrimitives.ReadUInt64BigEndian(container.AsSpan(8));
        if (version != PfdLayout.VersionV3 && version != PfdLayout.VersionV4)
            throw new SaveCryptoException($"The parameter file has version {version}; only versions 3 and 4 are known.");

        var capacity = BinaryPrimitives.ReadUInt64BigEndian(container.AsSpan(PfdLayout.HashTableOffset));
        var numReserved = BinaryPrimitives.ReadUInt64BigEndian(container.AsSpan(PfdLayout.HashTableOffset + 8));
        var numUsed = BinaryPrimitives.ReadUInt64BigEndian(container.AsSpan(PfdLayout.HashTableOffset + 16));

        var maxCapacity = (container.Length - PfdLayout.FirstEntryOffset) / (PfdLayout.EntryIndexSize + PfdLayout.HashSize);
        var maxReserved = (container.Length - PfdLayout.FirstEntryOffset) / PfdLayout.EntrySize;
        if (capacity == 0 || capacity > (ulong)maxCapacity || numUsed > numReserved || numReserved > (ulong)maxReserved)
            throw new SaveCryptoException("The parameter file describes a hash table that does not fit inside it.");

        var entryTableStart = PfdLayout.FirstEntryOffset + (int)capacity * PfdLayout.EntryIndexSize;
        var tableEnd = entryTableStart + (int)numReserved * PfdLayout.EntrySize + (int)capacity * PfdLayout.HashSize;
        if (tableEnd > container.Length)
            throw new SaveCryptoException("The parameter file describes entry and signature tables that do not fit inside it.");

        PfdCipher.DecryptSignatureBlock(
            container.AsSpan(PfdLayout.SignatureOffset, PfdLayout.SignatureSize),
            container.AsSpan(PfdLayout.HeaderKeyOffset, PfdLayout.HeaderKeySize));

        var storedHashKey = container.AsSpan(PfdLayout.HashKeyOffset, PfdLayout.HashKeySize);
        var pfd = new ParamPfd(container, capacity, numReserved, numUsed, entryTableStart, PfdCipher.RealHashKey(version, storedHashKey));
        pfd.CheckEntries();
        return pfd;
    }

    internal int DecryptBodies(string directory, byte[] expandedHashKey)
    {
        var fallback = DirectoryState(directory);
        var decrypted = 0;
        foreach (var (entry, name, path) in ManagedFiles(directory))
        {
            if (ResolveState(path, name, entry.FileSize, fallback) != BodyState.Encrypted) continue;
            SaveFileBody.Decrypt(path, entry.FileSize, PfdCipher.FileKey(expandedHashKey, entry.SealedKey));
            decrypted++;
        }
        return decrypted;
    }

    internal int EncryptBodies(string directory, byte[] expandedHashKey)
    {
        var fallback = DirectoryState(directory);
        var encrypted = 0;
        foreach (var (entry, name, path) in ManagedFiles(directory))
        {
            if (ResolveState(path, name, entry.FileSize, fallback) == BodyState.Encrypted) continue;
            entry.SetFileSize(SaveFileBody.Encrypt(path, PfdCipher.FileKey(expandedHashKey, entry.SealedKey)));
            encrypted++;
        }
        return encrypted;
    }

    internal SaveBodies DetectBodies(string directory)
    {
        var plaintext = 0;
        var encrypted = 0;
        foreach (var (entry, _, path) in ManagedFiles(directory))
        {
            if (!File.Exists(path)) continue;
            switch (SaveFileBody.StateOf(path, entry.FileSize))
            {
                case BodyState.Plaintext: plaintext++; break;
                case BodyState.Encrypted: encrypted++; break;
            }
        }
        return encrypted > 0 && plaintext == 0 ? SaveBodies.Encrypted : SaveBodies.Plaintext;
    }

    internal void RebuildHashes(string directory, byte[] expandedHashKey)
    {
        for (var index = 0; index < (int)numUsed; index++)
        {
            var entry = EntryAt(index);
            if (entry.IsParamSfo) continue;
            entry.SetFileHash(0, PfdCipher.Hmac(expandedHashKey, SaveFileBody.Read(directory, entry.Name)));
        }

        var defaultHash = PfdCipher.Hmac(realHashKey, ReadOnlySpan<byte>.Empty);
        for (var bucket = 0; bucket < (int)capacity; bucket++)
            if (HeadAt(bucket) >= capacity)
                WriteHash(SignatureSlot(bucket), defaultHash);

        for (var index = 0; index < (int)numUsed; index++)
        {
            var name = EntryAt(index).Name;
            WriteHash(SignatureSlot((int)BucketOf(name)), BucketHash(name));
        }

        WriteHash(PfdLayout.BottomHashOffset, PfdCipher.Hmac(realHashKey, container.AsSpan(signatureTableStart, (int)capacity * PfdLayout.HashSize)));
        WriteHash(PfdLayout.TopHashOffset, PfdCipher.Hmac(realHashKey, container.AsSpan(PfdLayout.HashTableOffset, PfdLayout.HashTableHeaderSize + (int)capacity * PfdLayout.EntryIndexSize)));
    }

    internal bool SaveIfChanged(string directory)
    {
        var path = Path.Combine(directory, PfdLayout.ContainerFileName);
        var output = (byte[])container.Clone();
        PfdCipher.EncryptSignatureBlock(
            output.AsSpan(PfdLayout.SignatureOffset, PfdLayout.SignatureSize),
            output.AsSpan(PfdLayout.HeaderKeyOffset, PfdLayout.HeaderKeySize));

        if (File.Exists(path) && File.ReadAllBytes(path).AsSpan().SequenceEqual(output)) return false;
        AtomicFile.Write(path, output);
        return true;
    }

    internal PfdValidation Validate(string directory, byte[] expandedHashKey)
    {
        var mirror = new ParamPfd((byte[])container.Clone(), capacity, numReserved, numUsed, entryTableStart, realHashKey);
        mirror.RebuildHashes(directory, expandedHashKey);

        return new PfdValidation(
            SaveFormat.Ps3Container,
            Matches(mirror, PfdLayout.TopHashOffset, PfdLayout.HashSize),
            Matches(mirror, PfdLayout.BottomHashOffset, PfdLayout.HashSize),
            Matches(mirror, signatureTableStart, (int)capacity * PfdLayout.HashSize),
            FileHashesMatch(mirror));
    }

    private BodyState DirectoryState(string directory)
    {
        var plaintext = 0;
        var encrypted = 0;
        foreach (var (entry, _, path) in ManagedFiles(directory))
        {
            if (!File.Exists(path)) continue;
            switch (SaveFileBody.StateOf(path, entry.FileSize))
            {
                case BodyState.Plaintext: plaintext++; break;
                case BodyState.Encrypted: encrypted++; break;
            }
        }
        return encrypted > 0 && plaintext == 0 ? BodyState.Encrypted : BodyState.Plaintext;
    }

    private static BodyState ResolveState(string path, string name, long declaredSize, BodyState fallback)
    {
        if (!File.Exists(path))
            throw new SaveCryptoException($"{name} is listed in the parameter file but is missing from the savedata folder.");
        var state = SaveFileBody.StateOf(path, declaredSize);
        return state == BodyState.Ambiguous ? fallback : state;
    }

    private IEnumerable<(PfdEntry Entry, string Name, string Path)> ManagedFiles(string directory)
    {
        for (var index = 0; index < (int)numUsed; index++)
        {
            var entry = EntryAt(index);
            if (entry.IsParamSfo) continue;
            var name = entry.Name;
            yield return (entry, name, System.IO.Path.Combine(directory, name));
        }
    }

    private byte[] BucketHash(string name)
    {
        using var hmac = IncrementalHash.CreateHMAC(HashAlgorithmName.SHA1, realHashKey);
        var index = HeadAt((int)BucketOf(name));
        var appended = false;
        var walked = 0UL;
        while (index < numReserved)
        {
            var entry = EntryAt((int)index);
            entry.AppendHashInput(hmac);
            appended = true;
            index = entry.AdditionalIndex;
            if (++walked > numReserved)
                throw new SaveCryptoException($"The parameter file entry chain for {name} never ends.");
        }
        if (!appended)
            throw new SaveCryptoException($"The parameter file entry for {name} cannot be reached from its hash bucket.");
        return hmac.GetHashAndReset();
    }

    private ulong BucketOf(string name)
    {
        ulong hash = 0;
        foreach (var character in name) hash = (hash << 5) - hash + (byte)character;
        return hash % capacity;
    }

    private PfdEntry EntryAt(int index) => new(container, entryTableStart + index * PfdLayout.EntrySize);

    private ulong HeadAt(int bucket) =>
        BinaryPrimitives.ReadUInt64BigEndian(container.AsSpan(PfdLayout.HashTableOffset + PfdLayout.HashTableHeaderSize + bucket * PfdLayout.EntryIndexSize));

    private void WriteHash(int offset, ReadOnlySpan<byte> hash) => hash.CopyTo(container.AsSpan(offset, PfdLayout.HashSize));

    private int SignatureSlot(int bucket) => signatureTableStart + bucket * PfdLayout.HashSize;

    private bool Matches(ParamPfd other, int offset, int length) =>
        container.AsSpan(offset, length).SequenceEqual(other.container.AsSpan(offset, length));

    private bool FileHashesMatch(ParamPfd other)
    {
        for (var index = 0; index < (int)numUsed; index++)
        {
            var entry = EntryAt(index);
            if (entry.IsParamSfo) continue;
            if (!entry.FileHash(0).SequenceEqual(other.EntryAt(index).FileHash(0))) return false;
        }
        return true;
    }

    private void CheckEntries()
    {
        for (var index = 0; index < (int)numUsed; index++)
        {
            var entry = EntryAt(index);
            if (entry.Name.Length == 0) throw new SaveCryptoException($"The parameter file entry {index} has no file name.");
            if (entry.FileSize < 0 || entry.FileSize > PfdLayout.MaxFileSize)
                throw new SaveCryptoException($"The parameter file entry for {entry.Name} declares an implausible size of {entry.FileSize} bytes.");
        }
    }
}
