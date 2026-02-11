namespace XmlSpecifiedGenerator.Tests.GeneratorTests;

public class NumericOptionsTests
{
    [Fact]
    public Task Generator_NumericOptions_Positive_GeneratesGreaterThanZero()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions = NumericOptions.Positive)]
                public int Quantity { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NumericOptions_Zero_GeneratesEqualsZero()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions = NumericOptions.Zero)]
                public int Value { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NumericOptions_Negative_GeneratesLessThanZero()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions = NumericOptions.Negative)]
                public int Value { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NumericOptions_PositiveOrZero_GeneratesGreaterOrEqualZero()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions = NumericOptions.Positive | NumericOptions.Zero)]
                public decimal Price { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NumericOptions_NegativeOrZero_GeneratesLessOrEqualZero()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions = NumericOptions.Negative | NumericOptions.Zero)]
                public int Value { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NumericOptions_NotZero_GeneratesNotEqualsZero()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions = NumericOptions.Positive | NumericOptions.Negative)]
                public int Value { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NumericOptions_All_AlwaysTrue()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions = NumericOptions.Positive | NumericOptions.Zero | NumericOptions.Negative)]
                public int Value { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }
}
