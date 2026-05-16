using IndependentWork24.Composite;

namespace IndependentWork24.Decorator;

public class CompressionDecorator : FileDecoratorBase
{
    private const double CompressionRatio = 0.70; 

    public CompressionDecorator(IFileSystemComponent component) : base(component) { }

    public override string Name => $"[ZIP] {Wrapped.Name}";

    public override long Size => (long)Math.Round(Wrapped.Size * CompressionRatio, MidpointRounding.AwayFromZero);

    public override void Display(int depth = 0)
    {
        string indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent} [Compressed -30%]");
        Wrapped.Display(depth + 1);
    }

    public static double Ratio => CompressionRatio;
}