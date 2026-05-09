using IndependentWork22.Components;

namespace IndependentWork22.Decorators
{
    public class TransactionLabelDecorator : TransactionDecorator
    {
        private readonly string _label;

        public TransactionLabelDecorator(IComponent component, string label)
            : base(component)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException("Label cannot be empty.", nameof(label));
            }

            _label = label.Trim();
        }

        public override void Display(string indent = "")
        {
            Console.WriteLine($"{indent}[Label: {_label}]");
            Component.Display(indent + "   ");
        }
    }
}
