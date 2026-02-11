namespace XmlSpecifiedGenerator.Tests.GeneratorTests;

public class ReferenceTypeTests
{
    [Fact]
    public Task Generator_ComplexObject_NoOptions_GeneratesNullCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public Address HomeAddress { get; set; }
            }

            public class Address
            {
                public string Street { get; set; }
                public string City { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_ObjectType_NoOptions_GeneratesNullCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public object Data { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_InterfaceType_NoOptions_GeneratesNullCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public IDisposable Resource { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NullableReferenceType_NoOptions_GeneratesNullCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public Person? Owner { get; set; }
            }

            public class Person
            {
                public string Name { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }
}
