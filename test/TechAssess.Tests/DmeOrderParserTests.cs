using TechAssess.src;

namespace TechAssess.Tests;

public class DmeOrderParserTests
{
    [Fact]
    public void Parse_NoteWithCpap_ReturnsCpapOrder()
    {
        // Arrange
        string note = "Patient requires CPAP with full face mask and humidifier. AHI > 20. Ordered by Dr. Smith.";

        // Act
        var order = DmeOrderParser.Parse(note);

        // Assert
        Assert.Equal("CPAP", order.DeviceType);
        Assert.Equal("full face", order.MaskType);
        Assert.Equal("humidifier", order.AddOns);
        Assert.Equal("AHI > 20", order.Qualifier);
        Assert.Equal("Dr. Smith", order.OrderingProvider);
        Assert.Null(order.OxygenLiters);
        Assert.Null(order.OxygenUsage);
    }

    [Fact]
    public void Parse_NoteWithOxygen_ReturnsOxygenOrder()
    {
        // Arrange
        string note = "Patient needs oxygen at 2.5 L for sleep and exertion. Ordered by Dr. Jones.";

        // Act
        var order = DmeOrderParser.Parse(note);

        // Assert
        Assert.Equal("Oxygen Tank", order.DeviceType);
        Assert.Equal("2.5 L", order.OxygenLiters);
        Assert.Equal("sleep and exertion", order.OxygenUsage);
        Assert.Equal("Dr. Jones", order.OrderingProvider);
        Assert.Null(order.MaskType);
        Assert.Null(order.AddOns);
        Assert.Equal("", order.Qualifier);
    }

    [Fact]
    public void Parse_NoteWithWheelchair_ReturnsWheelchairOrder()
    {
        // Arrange
        string note = "Patient requires a wheelchair. Ordered by Dr. Lee.";

        // Act
        var order = DmeOrderParser.Parse(note);

        // Assert
        Assert.Equal("Wheelchair", order.DeviceType);
        Assert.Equal("Dr. Lee", order.OrderingProvider);
        Assert.Null(order.MaskType);
        Assert.Null(order.AddOns);
        Assert.Equal("", order.Qualifier);
        Assert.Null(order.OxygenLiters);
        Assert.Null(order.OxygenUsage);
    }
}