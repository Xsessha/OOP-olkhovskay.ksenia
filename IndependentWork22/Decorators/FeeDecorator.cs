using IndependentWork22.Components;

namespace IndependentWork22.Decorators
{
    public class FeeDecorator : TransactionDecorator
    {
        private readonly decimal _fee;

        public FeeDecorator(IComponent component, decimal fee)
            : base(component)
        {
            if (fee < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fee), "Fee cannot be negative.");
            }

            _fee = fee;
        }

        public override decimal GetAmount()
        {
            return Component.GetAmount() + _fee;
        }

        public override void Display(string indent = "")
        {
            Console.WriteLine($"{indent}[FeeDecorator]");
            Component.Display(indent + "   ");
            Console.WriteLine($"{indent}   Fee: {_fee:0.00} USD");
            Console.WriteLine($"{indent}   Amount with fee: {GetAmount():0.00} USD");
        }
    }
}
