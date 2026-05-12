using IndependentWork23.Adapter;
using IndependentWork23.Facade;
using IndependentWork23.Proxy;

namespace IndependentWork23
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // ADAPTER

            Console.WriteLine("ADAPTER PATTERN");

            OldEventListener oldListener =
                new OldEventListener();

            IEventHandler adapter =
                new OldEventAdapter(oldListener);

            adapter.HandleEvent("UserLoggedIn");

            Console.WriteLine();

            // FACADE

            Console.WriteLine("FACADE PATTERN");

            EventProcessingFacade facade =
                new EventProcessingFacade();

            facade.HandleEvent("PaymentCompleted");

            Console.WriteLine();

            // PROXY

            Console.WriteLine("PROXY PATTERN");

            IEventPublisher publisher =
                new RateLimitEventPublisherProxy(3, cacheSeconds: 5);

            Console.WriteLine("Publish first event (real publisher)");
            publisher.Publish("OrderCreated");

            Thread.Sleep(1000);

            Console.WriteLine("Publish same event again (cache should be used)");
            publisher.Publish("OrderCreated");

            Thread.Sleep(1000);

            Console.WriteLine("Publish different event too quickly (blocked by rate limit)");
            publisher.Publish("OrderUpdated");

            Thread.Sleep(3500);

            Console.WriteLine("Publish another event after delay (real publisher)");
            publisher.Publish("OrderUpdated");

            Console.WriteLine();
        }
    }
}