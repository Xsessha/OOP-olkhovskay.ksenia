namespace IndependentWork23.Adapter
{
    public class OldEventAdapter : IEventHandler
    {
        private readonly OldEventListener _oldListener;

        public OldEventAdapter(OldEventListener oldListener)
        {
            _oldListener = oldListener;
        }

        public void HandleEvent(string eventMessage)
        {
            EventData data = new EventData(eventMessage);

            _oldListener.OnEvent(data);
        }
    }
}