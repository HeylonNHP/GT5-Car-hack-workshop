namespace PS3Pfd;

public readonly record struct PfdValidation(
    SaveFormat Format,
    bool TopHashValid,
    bool BottomHashValid,
    bool EntryHashesValid,
    bool FileHashesValid)
{
    public bool IsValid =>
        Format == SaveFormat.EmulatorPlaintext ||
        (TopHashValid && BottomHashValid && EntryHashesValid && FileHashesValid);
}
