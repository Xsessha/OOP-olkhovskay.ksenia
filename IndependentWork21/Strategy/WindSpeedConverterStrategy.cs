using System.Globalization;

namespace IndependentWork21.Strategy;

public class WindSpeedConverterStrategy : IDataProcessorStrategy
{
    public string Process(string data)
    {
        if (double.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out double s))
        {
            return $"Wind speed: {Format(s * 3.6)} km/h";
        }

        return "Invalid wind data";
    }

    private static string Format(double value)
    {
        return value.ToString("0.##", CultureInfo.InvariantCulture);
    }
}
