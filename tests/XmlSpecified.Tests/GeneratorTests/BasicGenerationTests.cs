namespace XmlSpecified.Tests.GeneratorTests;

public class BasicGenerationTests
{
    [Fact]
    public Task Generator_CreatesSpecifiedProperty_ForNullableInt()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public int? Age { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CreatesSpecifiedProperty_WithNumericOptions()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions.Positive)]
                public int Quantity { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CreatesSpecifiedProperty_WithNonNegativeNumericOptions()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions.Positive | NumericOptions.Zero)]
                public decimal Price { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CreatesSpecifiedProperty_WithStringOptions()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(StringOptions.NonWhitespace)]
                public string Name { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CreatesSpecifiedProperty_WithBoolOptions()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(BoolOptions.True)]
                public bool IsActive { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CreatesSpecifiedProperty_WithCollectionOptions()
    {
        // Arrange
        var source = """
            using XmlSpecified;
            using System.Collections.Generic;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(CollectionOptions.NonEmpty)]
                public List<string> Tags { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_HandlesNestedPartialClass()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class Outer
            {
                public partial class Inner
                {
                    [XmlSpecified]
                    public int? Value { get; set; }
                }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_HandlesGlobalNamespace()
    {
        // Arrange
        var source = """
            using XmlSpecified;

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
    public Task Generator_HandlesMultipleNamespaces()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace Outer
            {
                namespace Inner
                {
                    public partial class TestClass
                    {
                        [XmlSpecified]
                        public int? Value { get; set; }
                    }
                }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public void Generator_ProducesDeterministicOutput_ForMultipleProperties()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public int? Zebra { get; set; }

                [XmlSpecified]
                public int? Alpha { get; set; }

                [XmlSpecified]
                public int? Middle { get; set; }
            }

            public partial class TestClass2
            {
                [XmlSpecified]
                public int? Zebra { get; set; }

                [XmlSpecified]
                public int? Alpha { get; set; }

                [XmlSpecified]
                public int? Middle { get; set; }
            }
            """;

        // Act
        var generatedSource1 = GeneratorTestHelper.Verify(source);
        var generatedSource2 = GeneratorTestHelper.Verify(source);

        // Assert
        Assert.Equal(generatedSource1, generatedSource2);
    }

    [Fact]
    public Task Generator_HandlesMultipleNamespacesAndMultipleClasses()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace Outer
            {
                namespace Inner
                {
                    public partial class TestClass
                    {
                        [XmlSpecified]
                        public int? Value { get; set; }
                    }
                }

                public partial class TestClass2
                {
                    [XmlSpecified]
                    public string Value { get; set; }
                }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }
}
