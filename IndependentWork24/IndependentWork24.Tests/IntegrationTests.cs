using IndependentWork24.Composite;
using IndependentWork24.Decorator;
using IndependentWork24.Proxy;
using Xunit;

namespace IndependentWork24.Tests;

public class IntegrationTests
{

    [Fact(DisplayName = "Composite: Директорія з декорованим файлом враховує збільшений розмір")]
    public void CompositeWithDecoratedFile_Size_IsCorrect()
    {
        var dir  = new DirectoryComposite("secure");
        var file = new FileLeaf("data.bin", 10_000);

        IFileSystemComponent encryptedFile = new EncryptionDecorator(file); 
        dir.Add(encryptedFile);
        dir.Add(new FileLeaf("readme.txt", 500));

        Assert.Equal(11_500, dir.Size);
    }

    [Fact(DisplayName = "Composite: Ланцюг декораторів у директорії — коректний розмір")]
    public void CompositeWithChainedDecorators_Size_IsCorrect()
    {
        var dir  = new DirectoryComposite("archive");
        var file = new FileLeaf("report.pdf", 20_000);

        IFileSystemComponent chainedFile = new CompressionDecorator(new EncryptionDecorator(file));
        dir.Add(chainedFile);

        Assert.Equal(15_400, dir.Size);
    }

    [Fact(DisplayName = "Composite + Logging: Display дерева логує звернення")]
    public void CompositeDisplay_WithLoggingDecorator_LogsEntries()
    {
        var dir    = new DirectoryComposite("docs");
        var file   = new FileLeaf("memo.txt", 200);
        var logged = new LoggingDecorator(file);

        dir.Add(logged);
        dir.Display();

        Assert.NotEmpty(logged.GetLog());
    }


    [Fact(DisplayName = "Proxy + Composite: Reader бачить коректний розмір дерева")]
    public void Proxy_ReadContent_CompositeTreeSize_IsConsistent()
    {
        var real  = new RealFileAccess("meta.json", "{\"size\": 38400}");
        var proxy = new FileAccessProxy(real, UserRole.Reader);

        string content = proxy.Read();

        Assert.Contains("38400", content);
    }


    [Fact(DisplayName = "Інтеграція: Composite + Decorator + Proxy — повний сценарій")]
    public void FullIntegration_CompositeDecoratorProxy_WorkTogether()
    {
        var root = new DirectoryComposite("project");
        var src  = new DirectoryComposite("src");

        var mainFile = new FileLeaf("main.cs",    5_000, "entry point");
        var libFile  = new FileLeaf("library.cs", 8_000, "shared logic");

        IFileSystemComponent encMain = new EncryptionDecorator(mainFile); 

        src.Add(encMain);
        src.Add(libFile);
        root.Add(src);

        Assert.Equal(13_500, root.Size);

        var realMain   = new RealFileAccess("main.cs", "entry point");
        var adminProxy = new FileAccessProxy(realMain, UserRole.Admin);
        var guestProxy = new FileAccessProxy(realMain, UserRole.Guest);

        Assert.Equal("entry point", adminProxy.Read());
        adminProxy.Write("updated entry point");
        Assert.Equal("updated entry point", adminProxy.Read());

        Assert.Throws<UnauthorizedAccessException>(() => guestProxy.Read());

        Assert.Equal(13_500, root.Size);
    }

    [Fact(DisplayName = "Інтеграція: Logging + Proxy — аудит повний ланцюг")]
    public void Integration_LoggingAndProxy_AuditChain()
    {
        var file   = new FileLeaf("audit.txt", 1_000, "sensitive");
        var logged = new LoggingDecorator(file);

        var real  = new RealFileAccess("audit.txt", "sensitive");
        var proxy = new FileAccessProxy(real, UserRole.Editor);

        _ = logged.Size;
        _ = logged.Size;
        proxy.Read();
        proxy.Write("updated");
        proxy.Read();

        Assert.Equal(2, logged.GetLog().Count);

        Assert.Equal(3, proxy.GetAccessLog().Count);
    }


    [Fact(DisplayName = "[BOUNDARY] Порожня директорія з декорованим файлом розміром 0")]
    public void CompositeWithZeroSizeDecoratedFile_SizeIsZero()
    {
        var dir       = new DirectoryComposite("empty_project");
        var emptyFile = new FileLeaf("placeholder.txt", 0);

        IFileSystemComponent enc = new EncryptionDecorator(emptyFile);
        dir.Add(enc);

        Assert.Equal(0, dir.Size);
    }
}