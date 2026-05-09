using IndependentWork22.Components;

namespace IndependentWork22.Decorators
{
    public class CurrencyConverterDecorator : TransactionDecorator
    {
        private readonly decimal _exchangeRate;
        private readonly string _currency;

        public CurrencyConverterDecorator(
            IComponent component,
            decimal exchangeRate,
            string currency)
            : base(component)
        {
            if (exchangeRate <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(exchangeRate), "Exchange rate must be positive.");
            }

            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Currency code cannot be empty.", nameof(currency));
            }

            _exchangeRate = exchangeRate;
            _currency = currency.Trim().ToUpperInvariant();
        }

        public override decimal GetAmount()
        {
            return Component.GetAmount() * _exchangeRate;
        }

        public override void Display(string indent = "")
        {
            Console.WriteLine($"{indent}[CurrencyConverterDecorator]");
            Component.Display(indent + "   ");
            Console.WriteLine($"{indent}   Exchange rate: {_exchangeRate:0.00}");
            Console.WriteLine($"{indent}   Converted amount: {GetAmount():0.00} {_currency}");
        }
    }
}
