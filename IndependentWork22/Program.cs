using IndependentWork22.Components;
using IndependentWork22.Decorators;

namespace IndependentWork22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            IComponent transaction1 = new SingleTransaction("Laptop Purchase", 1200);
            IComponent transaction2 = new SingleTransaction("Phone Purchase", 800);
            IComponent transaction3 = new SingleTransaction("Headphones Purchase", 200);
            IComponent transaction4 = new SingleTransaction("Keyboard Purchase", 150);

            var electronicsBatch = new BatchTransaction("Electronics Batch");
            electronicsBatch.Add(transaction1);
            electronicsBatch.Add(transaction2);

            var accessoriesBatch = new BatchTransaction("Accessories Batch");
            accessoriesBatch.Add(transaction3);
            accessoriesBatch.Add(transaction4);

            var fullBatch = new BatchTransaction("Full Shopping Batch");
            fullBatch.Add(electronicsBatch);
            fullBatch.Add(accessoriesBatch);

            Console.WriteLine("1. ORIGINAL COMPOSITE STRUCTURE");
            fullBatch.Display();
            Console.WriteLine();

            IComponent transactionWithFee = new FeeDecorator(transaction1, 50);
            Console.WriteLine("2. DECORATED LEAF: FEE");
            transactionWithFee.Display();
            Console.WriteLine();

            IComponent convertedTransaction =
                new CurrencyConverterDecorator(
                    transaction2,
                    41.5m,
                    "UAH");

            Console.WriteLine("3. DECORATED LEAF: CURRENCY CONVERSION");
            convertedTransaction.Display();
            Console.WriteLine();

            IComponent decoratedBatch =
                new TransactionLabelDecorator(
                    new CurrencyConverterDecorator(
                        new FeeDecorator(fullBatch, 100),
                        41.5m,
                        "UAH"),
                    "Monthly expenses");

            Console.WriteLine("4. DECORATED COMPOSITE: LABEL + FEE + CONVERSION");
            decoratedBatch.Display();
            Console.WriteLine();

            accessoriesBatch.Remove(transaction4);

            Console.WriteLine("5. COMPOSITE AFTER REMOVE");
            fullBatch.Display();
        }
    }
}
