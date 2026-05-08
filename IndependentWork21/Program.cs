using System;
using IndependentWork21.Strategy;
using IndependentWork21.Observer;
using IndependentWork21.Services;

namespace IndependentWork21;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Weather System\n");

        var context = new DataContext(new CelsiusToFahrenheitStrategy());
        var publisher = new DataPublisher();
        var service = new ProcessingService(context, publisher);

        var consoleObserver = new ConsoleOutputObserver();
        var databaseObserver = new WeatherDatabaseObserver();

        consoleObserver.Subscribe(publisher);
        databaseObserver.Subscribe(publisher);

        Console.WriteLine("1) Celsius -> Fahrenheit:");
        service.Process("25");

        Console.WriteLine("\n2) Fahrenheit -> Celsius:");
        service.ChangeStrategy(new FahrenheitToCelsiusStrategy());
        service.Process("77");

        Console.WriteLine("\n3) Wind Speed:");
        service.ChangeStrategy(new WindSpeedConverterStrategy());
        service.Process("10");

        Console.WriteLine("\nManual Notify Observers");
        publisher.PublishDataProcessed("Manual weather notification");

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}