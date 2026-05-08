using IndependentWork21.Strategy;

namespace IndependentWork21.Factory;

public static class StrategyFactory
{
    public static IDataProcessorStrategy Create(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("Strategy type must be specified.", nameof(type));
        }

        return type.Trim().ToLowerInvariant() switch
        {
            "celsius" => new CelsiusToFahrenheitStrategy(),
            "fahrenheit" => new FahrenheitToCelsiusStrategy(),
            "wind" => new WindSpeedConverterStrategy(),
            _ => throw new ArgumentException($"Unknown strategy type: {type}", nameof(type))
        };
    }
}
