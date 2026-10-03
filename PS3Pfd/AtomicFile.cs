namespace PS3Pfd;

internal static class AtomicFile
{
    internal static void Write(string path, byte[] contents)
    {
        var fullPath = Path.GetFullPath(path);
        var temporary = Path.Combine(Path.GetDirectoryName(fullPath)!, Path.GetFileName(fullPath) + ".ps3pfd-part");
        try
        {
            File.WriteAllBytes(temporary, contents);
            File.Move(temporary, fullPath, overwrite: true);
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            Discard(temporary);
            throw new SaveCryptoException($"Could not write {fullPath}.", error);
        }
    }

    private static void Discard(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (IOException)
        {
        }
    }
}
