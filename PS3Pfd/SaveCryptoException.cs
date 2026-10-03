namespace PS3Pfd;

public sealed class SaveCryptoException : Exception
{
    public SaveCryptoException(string message) : base(message)
    {
    }

    public SaveCryptoException(string message, Exception inner) : base(message, inner)
    {
    }
}
