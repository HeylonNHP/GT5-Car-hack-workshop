using System.Buffers.Binary;
using System.Security.Cryptography;

namespace PS3Pfd;

internal static class PfdCipher
{
    internal static readonly byte[] SysconManagerKey = Convert.FromHexString("D413B89663E1FE9F75143D3BB4565274");
    internal static readonly byte[] KeygenKey = Convert.FromHexString("6B1ACEA246B745FD8F93763B920594CD53483B82");

    internal static void DecryptSignatureBlock(Span<byte> block, ReadOnlySpan<byte> headerKey)
    {
        using var aes = Aes.Create();
        aes.Key = SysconManagerKey;
        aes.DecryptCbc(block.ToArray(), headerKey.ToArray(), PaddingMode.None).CopyTo(block);
    }

    internal static void EncryptSignatureBlock(Span<byte> block, ReadOnlySpan<byte> headerKey)
    {
        using var aes = Aes.Create();
        aes.Key = SysconManagerKey;
        aes.EncryptCbc(block.ToArray(), headerKey.ToArray(), PaddingMode.None).CopyTo(block);
    }

    internal static byte[] ExpandSecureFileId(ReadOnlySpan<byte> secureFileId)
    {
        var hashKey = new byte[PfdLayout.HashKeySize];
        hashKey[0] = secureFileId[0];
        hashKey[1] = 0x0B;
        hashKey[2] = 0x0F;
        hashKey[3] = secureFileId[1];
        hashKey[4] = secureFileId[2];
        hashKey[5] = 0x0E;
        hashKey[6] = secureFileId[3];
        hashKey[7] = secureFileId[4];
        hashKey[8] = 0x0A;
        secureFileId[5..].CopyTo(hashKey.AsSpan(9));
        return hashKey;
    }

    internal static byte[] RealHashKey(ulong version, ReadOnlySpan<byte> storedHashKey) =>
        version == PfdLayout.VersionV4 ? Hmac(KeygenKey, storedHashKey) : storedHashKey.ToArray();

    internal static byte[] FileKey(ReadOnlySpan<byte> expandedHashKey, ReadOnlySpan<byte> sealedKey)
    {
        using var aes = Aes.Create();
        aes.Key = SysconManagerKey;
        var unwrapped = aes.DecryptCbc(sealedKey.ToArray(), expandedHashKey[..PfdLayout.KeySize].ToArray(), PaddingMode.None);
        return unwrapped[..PfdLayout.KeySize];
    }

    internal static byte[] Hmac(ReadOnlySpan<byte> key, ReadOnlySpan<byte> data) => HMACSHA1.HashData(key, data);

    internal static void Scramble(byte[] fileKey, byte[] data, bool decrypting)
    {
        using var aes = Aes.Create();
        aes.Key = fileKey;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.None;
        using var keystream = aes.CreateEncryptor();
        using var transformer = decrypting ? aes.CreateDecryptor() : aes.CreateEncryptor();

        var counter = new byte[PfdLayout.BlockSize];
        var mask = new byte[PfdLayout.BlockSize];
        var scratch = new byte[PfdLayout.BlockSize];
        var blocks = data.Length / PfdLayout.BlockSize;

        for (var block = 0; block < blocks; block++)
        {
            var offset = block * PfdLayout.BlockSize;
            Array.Clear(counter);
            BinaryPrimitives.WriteUInt64BigEndian(counter, (ulong)block);
            keystream.TransformBlock(counter, 0, PfdLayout.BlockSize, mask, 0);

            if (decrypting)
            {
                transformer.TransformBlock(data, offset, PfdLayout.BlockSize, scratch, 0);
                for (var i = 0; i < PfdLayout.BlockSize; i++) data[offset + i] = (byte)(scratch[i] ^ mask[i]);
            }
            else
            {
                for (var i = 0; i < PfdLayout.BlockSize; i++) scratch[i] = (byte)(data[offset + i] ^ mask[i]);
                transformer.TransformBlock(scratch, 0, PfdLayout.BlockSize, data, offset);
            }
        }
    }
}
