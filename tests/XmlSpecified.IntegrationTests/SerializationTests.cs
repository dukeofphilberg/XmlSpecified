using System.Xml.Serialization;

namespace XmlSpecified.IntegrationTests;

public class SerializationTests
{
    [Fact]
    public Task Order_WithNullableQuantity_Set_IncludesQuantityInresult()
    {
        // Arrange
        var order = new Order { OrderId = "ORD001", Quantity = 10 };

        // Act
        var result = SerializeToString(order);

        // Assert
        return Verify(result);
    }

    [Fact]
    public Task Order_WithNullableQuantity_Null_ExcludesQuantityFromresult()
    {
        // Arrange
        var order = new Order { OrderId = "ORD001", Quantity = null };

        // Act
        var result = SerializeToString(order);

        // Assert
        return Verify(result);
    }

    [Fact]
    public Task Order_WithDiscount_Positive_IncludesDiscountInresult()
    {
        // Arrange
        var order = new Order { OrderId = "ORD001", Discount = 15.5m };

        // Act
        var result = SerializeToString(order);

        // Assert
        return Verify(result);
    }

    [Fact]
    public Task Order_WithDiscount_Zero_ExcludesDiscountFromresult()
    {
        // Arrange
        var order = new Order { OrderId = "ORD001", Discount = 0 };

        // Act
        var result = SerializeToString(order);

        // Assert
        return Verify(result);
    }

    [Fact]
    public Task Order_WithNotes_NonEmpty_IncludesNotesInresult()
    {
        // Arrange
        var order = new Order { OrderId = "ORD001", Notes = "Please deliver to back door" };

        // Act
        var result = SerializeToString(order);

        // Assert
        return Verify(result);
    }

    [Fact]
    public Task Order_WithNotes_Empty_ExcludesNotesFromresult()
    {
        // Arrange
        var order = new Order { OrderId = "ORD001", Notes = "" };

        // Act
        var result = SerializeToString(order);

        // Assert
        return Verify(result);
    }

    [Fact]
    public Task Order_WithIsUrgent_True_IncludesIsUrgentInresult()
    {
        // Arrange
        var order = new Order { OrderId = "ORD001", IsUrgent = true };

        // Act
        var result = SerializeToString(order);

        // Assert
        return Verify(result);
    }

    [Fact]
    public Task Order_WithIsUrgent_False_ExcludesIsUrgentFromresult()
    {
        // Arrange
        var order = new Order { OrderId = "ORD001", IsUrgent = false };

        // Act
        var result = SerializeToString(order);

        // Assert
        return Verify(result);
    }

    private static string SerializeToString(Order order)
    {
        var serializer = new XmlSerializer(typeof(Order));
        using var writer = new StringWriter();
        serializer.Serialize(writer, order);
        return writer.ToString();
    }
}
