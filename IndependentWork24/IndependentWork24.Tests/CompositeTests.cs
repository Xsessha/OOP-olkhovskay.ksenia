using IndependentWork24.Composite;
using Xunit;

namespace IndependentWork24.Tests;

public class CompositeTests
{

    [Fact(DisplayName = "FileLeaf: Ім'я та розмір відповідають конструктору")]
    public void FileLeaf_NameAndSize_MatchConstructorArguments()
    {
        var file = new FileLeaf("data.csv", 4_096, "col1,col2");

        Assert.Equal("data.csv", file.Name);
        Assert.Equal(4_096, file.Size);
    }

    [Fact(DisplayName = "FileLeaf: GetContent повертає переданий вміст")]
    public void FileLeaf_GetContent_ReturnsInitialContent()
    {
        var file = new FileLeaf("notes.txt", 100, "Hello world");

        Assert.Equal("Hello world", file.GetContent());
    }

    [Fact(DisplayName = "DirectoryComposite: Порожня директорія має розмір 0")]
    public void Directory_Empty_SizeIsZero()
    {
        var dir = new DirectoryComposite("empty");

        Assert.Equal(0, dir.Size);
        Assert.Empty(dir.GetChildren());
    }

    [Fact(DisplayName = "DirectoryComposite: Розмір дорівнює сумі дочірніх файлів")]
    public void Directory_Size_EqualsSumOfChildren()
    {
        var dir = new DirectoryComposite("src");
        dir.Add(new FileLeaf("a.cs", 500));
        dir.Add(new FileLeaf("b.cs", 300));
        dir.Add(new FileLeaf("c.cs", 200));

        Assert.Equal(1_000, dir.Size);
    }

    [Fact(DisplayName = "DirectoryComposite: Вкладені директорії рекурсивно сумуються")]
    public void Directory_NestedDirectories_SizeIsRecursive()
    {
        var root = new DirectoryComposite("root");
        var sub  = new DirectoryComposite("sub");

        sub.Add(new FileLeaf("x.txt", 200));
        sub.Add(new FileLeaf("y.txt", 300));
        root.Add(sub);
        root.Add(new FileLeaf("z.txt", 100));

        Assert.Equal(600, root.Size);
    }

    [Fact(DisplayName = "DirectoryComposite: Remove видаляє дочірній елемент")]
    public void Directory_Remove_ChildIsRemovedAndSizeUpdates()
    {
        var dir  = new DirectoryComposite("dir");
        var file = new FileLeaf("temp.txt", 250);

        dir.Add(file);
        Assert.Single(dir.GetChildren());
        Assert.Equal(250, dir.Size);

        bool removed = dir.Remove(file);

        Assert.True(removed);
        Assert.Empty(dir.GetChildren());
        Assert.Equal(0, dir.Size);
    }


    [Fact(DisplayName = "[NEGATIVE] FileLeaf: Порожнє ім'я кидає ArgumentException")]
    public void FileLeaf_EmptyName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new FileLeaf("", 100));
    }

    [Fact(DisplayName = "[BOUNDARY] FileLeaf: Нульовий розмір — допустимий")]
    public void FileLeaf_ZeroSize_IsAllowed()
    {
        var file = new FileLeaf("empty.txt", 0);

        Assert.Equal(0, file.Size);
    }

    [Fact(DisplayName = "[NEGATIVE] FileLeaf: Від'ємний розмір кидає ArgumentOutOfRangeException")]
    public void FileLeaf_NegativeSize_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FileLeaf("bad.txt", -1));
    }
}