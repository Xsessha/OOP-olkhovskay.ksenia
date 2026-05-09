namespace IndependentWork22.Components
{
    public class SingleTransaction : IComponent
    {
        public string Description { get; }
        public decimal Amount { get; }

        public SingleTransaction(string description, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Transaction description cannot be empty.", nameof(description));
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Transaction amount cannot be negative.");
            }

            Description = description;
            Amount = amount;
        }

        public decimal GetAmount()
        {
            return Amount;
        }

        public void Display(string indent = "")
        {
            Console.WriteLine($"{indent}- Transaction: {Description} | Amount: {Amount:0.00} USD");
        }
    }
}
