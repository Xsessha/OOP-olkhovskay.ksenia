using IndependentWork21.Factory;
using IndependentWork21.Strategy;

namespace IndependentWork21.Tests;

public class StrategyTests
{
    [Theory]
    [InlineData("celsius", typeof(CelsiusToFahrenheitStrategy))]
    [InlineData("fahrenheit", typeof(FahrenheitToCelsiusStrategy))]
    [InlineData("wind", typeof(WindSpeedConverterStrategy))]
    public void Factory_ShouldCreateExpectedStrategyType(string strategyType, Type expectedType)
    {
        var strategy = StrategyFactory.Create(strategyType);

        Assert.IsType(expectedType, strategy);
        Assert.IsAssignableFrom<IDataProcessorStrategy>(strategy);
    }

    [Theory]
    [InlineData("0", "Celsius -> Fahrenheit: 32 F")]
    [InlineData("100", "Celsius -> Fahrenheit: 212 F")]
    [InlineData("-40", "Celsius -> Fahrenheit: -40 F")]
    [InlineData("-273.15", "Celsius -> Fahrenheit: -459.67 F")]
    public void CelsiusStrategy_ShouldCalculateExpectedResult(string celsius, string expected)
    {
        var strategy = StrategyFactory.Create("celsius");

        var result = strategy.Process(celsius);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("32", "Fahrenheit -> Celsius: 0 C")]
    [InlineData("212", "Fahrenheit -> Celsius: 100 C")]
    [InlineData("-40", "Fahrenheit -> Celsius: -40 C")]
    public void FahrenheitStrategy_ShouldCalculateExpectedResult(string fahrenheit, string expected)
    {
        var strategy = StrategyFactory.Create("fahrenheit");

        var result = strategy.Process(fahrenheit);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0", "Wind speed: 0 km/h")]
    [InlineData("0.1", "Wind speed: 0.36 km/h")]
    [InlineData("10", "Wind speed: 36 km/h")]
    [InlineData("50", "Wind speed: 180 km/h")]
    public void WindStrategy_ShouldCalculateExpectedResult(string mps, string expected)
    {
        var strategy = StrategyFactory.Create("wind");

        var result = strategy.Process(mps);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void DataContext_SetStrategy_ShouldSwitchRuntimeBehavior()
    {
        var context = new DataContext(StrategyFactory.Create("celsius"));

        var firstResult = context.ExecuteProcessing("0");
        context.SetStrategy(StrategyFactory.Create("fahrenheit"));
        var secondResult = context.ExecuteProcessing("32");

        Assert.Equal("Celsius -> Fahrenheit: 32 F", firstResult);
        Assert.Equal("Fahrenheit -> Celsius: 0 C", secondResult);
    }
}
