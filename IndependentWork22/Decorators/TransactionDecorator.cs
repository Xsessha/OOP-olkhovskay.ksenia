using IndependentWork22.Components;

namespace IndependentWork22.Decorators
{
    public abstract class TransactionDecorator : IComponent
    {
        protected IComponent Component { get; }

        protected TransactionDecorator(IComponent component)
        {
            Component = component ?? throw new ArgumentNullException(nameof(component));
        }

        public virtual decimal GetAmount()
        {
            return Component.GetAmount();
        }

        public virtual void Display(string indent = "")
        {
            Component.Display(indent);
        }
    }
}
