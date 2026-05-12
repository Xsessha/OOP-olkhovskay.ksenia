using System;

namespace IndependentWork23.Facade
{
    public class EventSource
    {
        public void RaiseEvent(string eventName)
        {
            Console.WriteLine($"Event raised: {eventName}");
        }
    }
}