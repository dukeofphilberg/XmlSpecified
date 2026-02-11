namespace XmlSpecified.Tests.GeneratorTests;

public class NullableTypeTests
{
    [Fact]
    public Task Generator_NullableInt_UsesHasValue()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public int? Value { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NullableDateTime_UsesHasValue()
    {
        // Arrange
        var source = """
            using XmlSpecified;
            using System;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public DateTime? DateValue { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NullableDecimal_UsesHasValue()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public decimal? Price { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NullableBool_UsesHasValue()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public bool? IsActive { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NullableInt_WithNumericOptions_UsesCombinedCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions = NumericOptions.Positive)]
                public int? Value { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NullableDecimal_WithNonNegativeNumericOptions_UsesCombinedCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions = NumericOptions.Positive | NumericOptions.Zero)]
                public decimal? Price { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }
}
