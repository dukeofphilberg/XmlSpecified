namespace XmlSpecifiedGenerator.Tests.GeneratorTests;

public class BoolOptionsTests
{
    [Fact]
    public Task Generator_BoolOptions_True_GeneratesPropertyCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

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
    public Task Generator_BoolOptions_False_GeneratesNegatedPropertyCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(BoolOptions = BoolOptions.False)]
                public bool IsDeleted { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }
}
