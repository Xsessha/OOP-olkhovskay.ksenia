using IndependentWork24.Composite;
using IndependentWork24.Decorator;
using Xunit;

namespace IndependentWork24.Tests;
public class DecoratorTests
{
    private static FileLeaf MakeFile(long size = 10_000) =>
        new("base.txt", size, "sample content");



    [Fact(DisplayName = "EncryptionDecorator: Розмір збільшується на 10%")]
    public void Encryption_Size_IncreasedByTenPercent()
    {
        var file      = MakeFile(10_000);
        var encrypted = new EncryptionDecorator(file);

        Assert.Equal(11_000, encrypted.Size);
    }

    [Fact(DisplayName = "EncryptionDecorator: Ім'я має префікс [ENC]")]
    public void Encryption_Name_HasEncPrefix()
    {
        var file      = MakeFile();
        var encrypted = new EncryptionDecorator(file);

        Assert.StartsWith("[ENC]", encrypted.Name);
    }

    [Fact(DisplayName = "CompressionDecorator: Розмір зменшується на 30%")]
    public void Compression_Size_DecreasedByThirtyPercent()
    {
        var file       = MakeFile(10_000);
        var compressed = new CompressionDecorator(file);

        Assert.Equal(7_000, compressed.Size); 
    }

    [Fact(DisplayName = "CompressionDecorator: Ім'я має префікс [ZIP]")]
    public void Compression_Name_HasZipPrefix()
    {
        var file       = MakeFile();
        var compressed = new CompressionDecorator(file);

        Assert.StartsWith("[ZIP]", compressed.Name);
    }

    [Fact(DisplayName = "Ланцюг: Encrypt→Compress правильно обчислює розмір")]
    public void ChainedDecorators_EncryptThenCompress_CorrectSize()
    {
        var file  = MakeFile(10_000);
    
        var chained = new CompressionDecorator(new EncryptionDecorator(file));

        Assert.Equal(7_700, chained.Size);
    }

    [Fact(DisplayName = "LoggingDecorator: GetSize фіксує запис у журнал")]
    public void Logging_GetSize_AddsLogEntry()
    {
        var file   = MakeFile();
        var logged = new LoggingDecorator(file);

        _ = logged.Size;
        _ = logged.Size;

        Assert.Equal(2, logged.GetLog().Count);
    }

    [Fact(DisplayName = "LoggingDecorator: Display фіксує запис у журнал")]
    public void Logging_Display_AddsLogEntry()
    {
        var file   = MakeFile();
        var logged = new LoggingDecorator(file);

        logged.Display();

        Assert.Single(logged.GetLog());
    }

    [Fact(DisplayName = "LoggingDecorator: ClearLog очищає всі записи")]
    public void Logging_ClearLog_RemovesAllEntries()
    {
        var file   = MakeFile();
        var logged = new LoggingDecorator(file);
        _ = logged.Size;
        _ = logged.Size;

        logged.ClearLog();

        Assert.Empty(logged.GetLog());
    }

    [Fact(DisplayName = "EncryptionDecorator: Encrypt/Decrypt — симетрична операція")]
    public void Encryption_EncryptDecrypt_IsSymmetric()
    {
        const string original = "Hello World";
        string cipher = EncryptionDecorator.Encrypt(original);
        string result = EncryptionDecorator.Decrypt(cipher);

        Assert.NotEqual(original, cipher);  
        Assert.Equal(original, result);     
    }


    [Fact(DisplayName = "[NEGATIVE] FileDecoratorBase: null-компонент кидає ArgumentNullException")]
    public void Decorator_NullComponent_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new EncryptionDecorator(null!));
        Assert.Throws<ArgumentNullException>(() => new CompressionDecorator(null!));
        Assert.Throws<ArgumentNullException>(() => new LoggingDecorator(null!));
    }

    [Fact(DisplayName = "[BOUNDARY] EncryptionDecorator: Порожній рядок шифрується як порожній")]
    public void Encryption_EmptyString_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, EncryptionDecorator.Encrypt(string.Empty));
        Assert.Equal(string.Empty, EncryptionDecorator.Decrypt(string.Empty));
    }
}