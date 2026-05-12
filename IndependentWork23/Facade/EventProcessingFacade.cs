namespace IndependentWork23.Facade
{
    public class EventProcessingFacade
    {
        private readonly EventSource _eventSource;
        private readonly Logger _logger;
        private readonly Notifier _notifier;

        public EventProcessingFacade()
        {
            _eventSource = new EventSource();
            _logger = new Logger();
            _notifier = new Notifier();
        }

        public void HandleEvent(string eventName)
        {
            _eventSource.RaiseEvent(eventName);

            _logger.Log($"Event processed: {eventName}");

            _notifier.Notify($"Notification sent for event: {eventName}");
        }
    }
}