namespace XmlSpecifiedGenerator.Tests.GeneratorTests;

public class StringOptionsTests
{
    [Fact]
    public Task Generator_StringOptions_NonWhitespace_GeneratesIsNullOrWhitespaceCheck()
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
    public Task Generator_StringOptions_NonEmpty_GeneratesIsNullOrEmptyCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(StringOptions.NonEmpty)]
                public string Description { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_StringOptions_NonNull_GeneratesNullCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(StringOptions.NonNull)]
                public string Value { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }
}
