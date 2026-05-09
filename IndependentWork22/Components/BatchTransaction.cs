namespace IndependentWork22.Components
{
    public class BatchTransaction : IComponent
    {
        private readonly List<IComponent> _transactions = new();

        public string BatchName { get; }
        public IReadOnlyList<IComponent> Transactions => _transactions;

        public BatchTransaction(string batchName)
        {
            if (string.IsNullOrWhiteSpace(batchName))
            {
                throw new ArgumentException("Batch name cannot be empty.", nameof(batchName));
            }

            BatchName = batchName;
        }

        public void Add(IComponent component)
        {
            _transactions.Add(component ?? throw new ArgumentNullException(nameof(component)));
        }

        public void Remove(IComponent component)
        {
            _transactions.Remove(component);
        }

        public decimal GetAmount()
        {
            decimal total = 0;

            foreach (var transaction in _transactions)
            {
                total += transaction.GetAmount();
            }

            return total;
        }

        public void Display(string indent = "")
        {
            Console.WriteLine($"{indent}+ Batch: {BatchName}");

            foreach (var transaction in _transactions)
            {
                transaction.Display(indent + "   ");
            }

            Console.WriteLine($"{indent}  Total Amount: {GetAmount():0.00} USD");
        }
    }
}
