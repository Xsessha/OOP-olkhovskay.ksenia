using IndependentWork24.Proxy;
using Xunit;

namespace IndependentWork24.Tests;


public class ProxyTests
{
    private static RealFileAccess MakeReal(string content = "initial")
        => new("test.txt", content);


    [Fact(DisplayName = "Admin: може читати і записувати")]
    public void Admin_CanReadAndWrite()
    {
        var proxy = new FileAccessProxy(MakeReal(), UserRole.Admin);

        Assert.True(proxy.CanRead);
        Assert.True(proxy.CanWrite);
        Assert.Equal("initial", proxy.Read());

        proxy.Write("updated");
        Assert.Equal("updated", proxy.Read());
    }

    [Fact(DisplayName = "Editor: може читати і записувати")]
    public void Editor_CanReadAndWrite()
    {
        var proxy = new FileAccessProxy(MakeReal("data"), UserRole.Editor);

        proxy.Write("new data");
        Assert.Equal("new data", proxy.Read());
    }

    [Fact(DisplayName = "Reader: може читати, але не записувати")]
    public void Reader_CanReadButNotWrite()
    {
        var proxy = new FileAccessProxy(MakeReal("secret"), UserRole.Reader);

        Assert.True(proxy.CanRead);
        Assert.False(proxy.CanWrite);
        Assert.Equal("secret", proxy.Read());
    }

    [Fact(DisplayName = "Proxy: журнал фіксує успішні операції")]
    public void Proxy_SuccessfulOperations_LoggedCorrectly()
    {
        var proxy = new FileAccessProxy(MakeReal(), UserRole.Admin);

        proxy.Read();
        proxy.Write("x");
        proxy.Read();

        Assert.Equal(3, proxy.GetAccessLog().Count);
        Assert.All(proxy.GetAccessLog(), e => Assert.Contains("[OK]", e));
    }


    [Fact(DisplayName = "[NEGATIVE] Guest: читання кидає UnauthorizedAccessException")]
    public void Guest_Read_ThrowsUnauthorizedAccess()
    {
        var proxy = new FileAccessProxy(MakeReal(), UserRole.Guest);

        var ex = Assert.Throws<UnauthorizedAccessException>(() => proxy.Read());
        Assert.Contains("Guest", ex.Message);
    }

    [Fact(DisplayName = "[NEGATIVE] Reader: запис кидає UnauthorizedAccessException")]
    public void Reader_Write_ThrowsUnauthorizedAccess()
    {
        var proxy = new FileAccessProxy(MakeReal(), UserRole.Reader);

        var ex = Assert.Throws<UnauthorizedAccessException>(() => proxy.Write("hack"));
        Assert.Contains("Reader", ex.Message);
    }

    [Fact(DisplayName = "[NEGATIVE] Proxy: null замість RealFileAccess кидає ArgumentNullException")]
    public void Proxy_NullRealAccess_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new FileAccessProxy(null!, UserRole.Admin));
    }

    [Fact(DisplayName = "[BOUNDARY] Guest: заборонений доступ фіксується в журналі")]
    public void Guest_DeniedAccess_IsLogged()
    {
        var proxy = new FileAccessProxy(MakeReal(), UserRole.Guest);

        try { proxy.Read(); } catch (UnauthorizedAccessException) { /* expected */ }
        try { proxy.Write("x"); } catch (UnauthorizedAccessException) { /* expected */ }

        Assert.Equal(2, proxy.GetAccessLog().Count);
        Assert.All(proxy.GetAccessLog(), e => Assert.Contains("[DENIED]", e));
    }

    [Fact(DisplayName = "RealFileAccess: Write(null) кидає ArgumentNullException")]
    public void RealFileAccess_WriteNull_ThrowsArgumentNullException()
    {
        var real = MakeReal();

        Assert.Throws<ArgumentNullException>(() => real.Write(null!));
    }
}