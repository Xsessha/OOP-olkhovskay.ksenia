using System;

namespace IndependentWork23.Facade
{
    public class Logger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[LOG]: {message}");
        }
    }
}