using System;

namespace IndependentWork23.Adapter
{
    public class OldEventListener
    {
        public void OnEvent(EventData data)
        {
            Console.WriteLine(
                $"[OLD SYSTEM] Event: {data.EventName} | Time: {data.Timestamp}");
        }
    }
}