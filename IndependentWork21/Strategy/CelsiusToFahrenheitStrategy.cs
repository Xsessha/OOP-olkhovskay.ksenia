using System.Globalization;

namespace IndependentWork21.Strategy;

public class CelsiusToFahrenheitStrategy : IDataProcessorStrategy
{
    public string Process(string data)
    {
        if (double.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out double c))
        {
            return $"Celsius -> Fahrenheit: {Format((c * 9 / 5) + 32)} F";
        }

        return "Invalid temperature data";
    }

    private static string Format(double value)
    {
        return value.ToString("0.##", CultureInfo.InvariantCulture);
    }
}
