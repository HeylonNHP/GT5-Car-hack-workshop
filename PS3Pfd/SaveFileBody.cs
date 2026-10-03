namespace PS3Pfd;

internal enum BodyState
{
    Plaintext,
    Encrypted,
    Ambiguous
}

internal static class SaveFileBody
{
    internal static long AlignedLength(long length) =>
        (length + PfdLayout.BlockSize - 1) / PfdLayout.BlockSize * PfdLayout.BlockSize;

    internal static BodyState StateOf(string path, long declaredSize)
    {
        var onDisk = new FileInfo(path).Length;
        var aligned = AlignedLength(declaredSize);
        if (onDisk == declaredSize && onDisk != aligned) return BodyState.Plaintext;
        if (onDisk == aligned && onDisk != declaredSize) return BodyState.Encrypted;
        return BodyState.Ambiguous;
    }

    internal static byte[] Read(string directory, string fileName) => Read(Path.Combine(directory, fileName));

    internal static void Decrypt(string path, long declaredSize, byte[] fileKey)
    {
        var ciphertext = ReadAligned(path, AlignedLength(declaredSize));
        PfdCipher.Scramble(fileKey, ciphertext, decrypting: true);
        AtomicFile.Write(path, ciphertext[..(int)declaredSize]);
    }

    internal static long Encrypt(string path, byte[] fileKey)
    {
        var plaintext = Read(path);
        var padded = new byte[AlignedLength(plaintext.Length)];
        plaintext.CopyTo(padded, 0);
        PfdCipher.Scramble(fileKey, padded, decrypting: false);
        AtomicFile.Write(path, padded);
        return plaintext.Length;
    }

    internal static byte[] Read(string path)
    {
        try
        {
            return File.ReadAllBytes(path);
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            throw new SaveCryptoException($"Could not read {path}.", error);
        }
    }

    private static byte[] ReadAligned(string path, long length)
    {
        if (length > int.MaxValue)
            throw new SaveCryptoException($"{path} is too large for the parameter file entry that describes it.");
        if (new FileInfo(path).Length < length)
            throw new SaveCryptoException($"{path} is shorter than the parameter file entry that describes it.");
        try
        {
            using var stream = File.OpenRead(path);
            var data = new byte[(int)length];
            stream.ReadExactly(data);
            return data;
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            throw new SaveCryptoException($"Could not read {path}.", error);
        }
    }
}
