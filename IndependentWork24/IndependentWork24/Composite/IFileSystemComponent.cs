namespace IndependentWork24.Composite;

public interface IFileSystemComponent
{
    string Name { get; }

    long Size { get; }

    void Display(int depth = 0);
}