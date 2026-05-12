using System;

namespace IndependentWork23.Proxy
{
    public class RealEventPublisher : IEventPublisher
    {
        public string Publish(string eventName)
        {
            string result = $"[PUBLISHER] Event published: {eventName}";
            Console.WriteLine(result);
            return result;
        }
    }
}