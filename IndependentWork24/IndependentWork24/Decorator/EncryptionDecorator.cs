using IndependentWork24.Composite;

namespace IndependentWork24.Decorator;

public class EncryptionDecorator : FileDecoratorBase
{
    private const double OverheadFactor = 1.10; 
    public EncryptionDecorator(IFileSystemComponent component) : base(component) { }

    public override string Name => $"[ENC] {Wrapped.Name}";

    public override long Size => (long)Math.Round(Wrapped.Size * OverheadFactor, MidpointRounding.AwayFromZero);

    public override void Display(int depth = 0)
    {
        string indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent} [Encrypted +10%]");
        Wrapped.Display(depth + 1);
    }

    public static string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return string.Empty;
        return new string(plainText.Select(c => char.IsLetter(c)
            ? (char)(c + 3)
            : c).ToArray());
    }

    public static string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return string.Empty;
        return new string(cipherText.Select(c => char.IsLetter(c)
            ? (char)(c - 3)
            : c).ToArray());
    }
}