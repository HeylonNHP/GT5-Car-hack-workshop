namespace PS3Pfd;

public sealed class SaveKeys
{
    public static SaveKeys Gt5 { get; } = new(Convert.FromHexString("BDBD2EB72D82473DBE09F1B552A93FE6"));

    public SaveKeys(byte[] secureFileId)
    {
        ArgumentNullException.ThrowIfNull(secureFileId);
        if (secureFileId.Length != PfdLayout.SecureFileIdSize)
            throw new ArgumentException($"A secure file id is {PfdLayout.SecureFileIdSize} bytes.", nameof(secureFileId));
        SecureFileId = (byte[])secureFileId.Clone();
    }

    public byte[] SecureFileId { get; }

    public byte[] HashKey() => PfdCipher.ExpandSecureFileId(SecureFileId);
}
