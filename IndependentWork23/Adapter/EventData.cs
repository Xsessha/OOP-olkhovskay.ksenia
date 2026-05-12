namespace IndependentWork23.Adapter
{
    public class EventData
    {
        public string EventName { get; set; }
        public DateTime Timestamp { get; set; }

        public EventData(string eventName)
        {
            EventName = eventName;
            Timestamp = DateTime.Now;
        }
    }
}