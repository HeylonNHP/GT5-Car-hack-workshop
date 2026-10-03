namespace PS3Pfd;

public readonly record struct PfdValidation(
    SaveFormat Format,
    bool? TopHashValid,
    bool? BottomHashValid,
    bool? EntryHashesValid,
    bool? FileHashesValid,
    string? Problem)
{
    public bool IsValid =>
        Problem is null &&
        (Format == SaveFormat.EmulatorPlaintext ||
         (TopHashValid == true && BottomHashValid == true && EntryHashesValid == true && FileHashesValid == true));
}
