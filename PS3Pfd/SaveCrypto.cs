namespace PS3Pfd;

public static class SaveCrypto
{
    public static bool HasContainer(string savedataDirectory) =>
        File.Exists(ContainerPath(savedataDirectory));

    public static SaveBodies DetectBodies(string savedataDirectory)
    {
        var pfd = Open(savedataDirectory);
        return pfd is null ? SaveBodies.Plaintext : pfd.DetectBodies(savedataDirectory);
    }

    public static SaveOperation Decrypt(string savedataDirectory, SaveKeys? keys = null)
    {
        var pfd = Open(savedataDirectory);
        if (pfd is null) return Untouched();

        var expandedHashKey = (keys ?? SaveKeys.Gt5).HashKey();
        var transformed = pfd.DecryptBodies(savedataDirectory, expandedHashKey);
        pfd.RebuildHashes(savedataDirectory, expandedHashKey);
        pfd.SaveIfChanged(savedataDirectory);
        return new SaveOperation(SaveFormat.Ps3Container, SaveBodies.Plaintext, transformed);
    }

    public static SaveOperation Encrypt(string savedataDirectory, SaveKeys? keys = null)
    {
        var pfd = Open(savedataDirectory);
        if (pfd is null) return Untouched();

        var expandedHashKey = (keys ?? SaveKeys.Gt5).HashKey();
        var transformed = pfd.EncryptBodies(savedataDirectory, expandedHashKey);
        pfd.RebuildHashes(savedataDirectory, expandedHashKey);
        pfd.SaveIfChanged(savedataDirectory);
        return new SaveOperation(SaveFormat.Ps3Container, SaveBodies.Encrypted, transformed);
    }

    public static PfdValidation Validate(string savedataDirectory, SaveKeys? keys = null)
    {
        if (!HasContainer(savedataDirectory))
            return new PfdValidation(SaveFormat.EmulatorPlaintext, null, null, null, null, null);

        try
        {
            return Open(savedataDirectory)!.Validate(savedataDirectory, (keys ?? SaveKeys.Gt5).HashKey());
        }
        catch (SaveCryptoException error)
        {
            return new PfdValidation(SaveFormat.Ps3Container, null, null, null, null, error.Message);
        }
    }

    public static string SavedataDirectoryOf(string saveFilePath) =>
        Path.GetDirectoryName(Path.GetFullPath(saveFilePath)) ??
        throw new SaveCryptoException($"{saveFilePath} does not live inside a savedata folder.");

    private static SaveOperation Untouched() => new(SaveFormat.EmulatorPlaintext, SaveBodies.Plaintext, 0);

    private static string ContainerPath(string savedataDirectory) => Path.Combine(savedataDirectory, PfdLayout.ContainerFileName);

    private static ParamPfd? Open(string savedataDirectory)
    {
        var path = ContainerPath(savedataDirectory);
        if (!File.Exists(path)) return null;

        var container = SaveFileBody.Read(path);
        try
        {
            return ParamPfd.Parse(container);
        }
        catch (SaveCryptoException error)
        {
            throw new SaveCryptoException($"{path} is not a usable PS3 parameter file: {error.Message}", error);
        }
    }
}
