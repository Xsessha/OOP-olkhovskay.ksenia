using System.Globalization;

namespace IndependentWork21.Strategy;

public class FahrenheitToCelsiusStrategy : IDataProcessorStrategy
{
    public string Process(string data)
    {
        if (double.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out double f))
        {
            return $"Fahrenheit -> Celsius: {Format((f - 32) * 5 / 9)} C";
        }

        return "Invalid temperature data";
    }

    private static string Format(double value)
    {
        return value.ToString("0.##", CultureInfo.InvariantCulture);
    }
}
