namespace XmlSpecified.Tests.GeneratorTests;

/// <summary>
/// Tests that validate the examples in quickstart.md compile and generate correctly.
/// </summary>
public class QuickstartExamplesTests
{
    [Fact]
    public Task Quickstart_BasicUsage_GeneratesCorrectCode()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            public partial class Order
            {
                [XmlSpecified(NumericOptions = NumericOptions.Positive)]
                public int Quantity { get; set; }

                [XmlSpecified(StringOptions = StringOptions.NonWhitespace)]
                public string CustomerName { get; set; }

                [XmlSpecified]
                public System.DateTime? ShipDate { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Quickstart_NumericPositiveOrZero_GeneratesCorrectCode()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions = NumericOptions.Positive | NumericOptions.Zero)]
                public int Quantity { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Quickstart_BoolTrue_GeneratesCorrectCode()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            public partial class TestClass
            {
                [XmlSpecified(BoolOptions = BoolOptions.True)]
                public bool IsActive { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Quickstart_CollectionNonEmpty_GeneratesCorrectCode()
    {
        // Arrange
        var source = """
            using XmlSpecified;
            using System.Collections.Generic;

            public partial class TestClass
            {
                [XmlSpecified(CollectionOptions = CollectionOptions.NonEmpty)]
                public List<string> Tags { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Quickstart_NullableWithNumericOptions_GeneratesCorrectCode()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions = NumericOptions.Positive)]
                public int? Count { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }
}
