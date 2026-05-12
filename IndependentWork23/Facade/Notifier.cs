using System;

namespace IndependentWork23.Facade
{
    public class Notifier
    {
        public void Notify(string message)
        {
            Console.WriteLine($"[NOTIFICATION]: {message}");
        }
    }
}