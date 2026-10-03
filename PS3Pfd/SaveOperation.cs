namespace PS3Pfd;

public readonly record struct SaveOperation(SaveFormat Format, SaveBodies Bodies, int FilesTransformed)
{
    public bool ChangedAnything => FilesTransformed > 0;
}
