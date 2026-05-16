using IndependentWork24.Composite;

namespace IndependentWork24.Decorator;

public abstract class FileDecoratorBase : IFileSystemComponent
{
    protected readonly IFileSystemComponent Wrapped;

    protected FileDecoratorBase(IFileSystemComponent component)
    {
        Wrapped = component ?? throw new ArgumentNullException(nameof(component));
    }

    public virtual string Name => Wrapped.Name;
    public virtual long Size => Wrapped.Size;

    public virtual void Display(int depth = 0) => Wrapped.Display(depth);
}