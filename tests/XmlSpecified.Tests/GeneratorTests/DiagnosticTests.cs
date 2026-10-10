namespace XmlSpecified.Tests.GeneratorTests;

public class DiagnosticTests
{
    [Fact]
    public Task Generator_NonPartialClass_ReportsXSG001()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public class TestClass  // Missing partial keyword
            {
                [XmlSpecified]
                public int? Value { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public void Generator_NonPartialOuterClass_ReportsXSG001()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public class Outer
            {
                public partial class Inner
                {
                    [XmlSpecified]
                    public int? Value { get; set; }
                }
            }
            """;

        // Act
        var (result, errors) = GeneratorTestHelper.RunAndCompile(source);

        // Assert
        Assert.Equal(["XSG001"], GeneratorTestHelper.GeneratorDiagnosticIds(result));
        Assert.Empty(errors);
    }

    [Fact]
    public Task Generator_DuplicateSpecifiedProperty_ReportsXSG002()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public int? Value { get; set; }

                // This will conflict with the generated ValueSpecified
                public bool ValueSpecified => true;
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Theory]
    [InlineData("public bool ValueSpecified => true;")]
    [InlineData("public bool ValueSpecified;")]
    [InlineData("public bool ValueSpecified() => true;")]
    public void Generator_ExistingSpecifiedMember_ReportsXSG002AndSkipsGeneration(string member)
    {
        // Arrange
        var source = $$"""
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public int? Value { get; set; }

                {{member}}
            }
            """;

        // Act
        var (result, errors) = GeneratorTestHelper.RunAndCompile(source);

        // Assert
        Assert.Equal(["XSG002"], GeneratorTestHelper.GeneratorDiagnosticIds(result));
        Assert.Empty(errors);
        Assert.Empty(result.GeneratedTrees);
    }

    [Fact]
    public Task Generator_ReadOnlyProperty_ReportsXSG003()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public int? Value { get; }  // No setter
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_StaticProperty_ReportsXSG004()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]
                public static int? Value { get; set; }  // Static property
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_ConflictingOptions_ReportsXSG005()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(NumericOptions.Positive)]
                public string Name { get; set; }  // Option for the wrong type, StringOptions is missing
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_MissingRequiredOption_NonNullableInt_ReportsXSG005()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]  // Missing NumericOptions for non-nullable int
                public int Quantity { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_MissingRequiredOption_String_ReportsXSG005()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]  // Missing StringOptions for string
                public string Name { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_MissingRequiredOption_Bool_ReportsXSG005()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]  // Missing BoolOptions for bool
                public bool IsActive { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_MissingRequiredOption_Collection_ReportsXSG005()
    {
        // Arrange
        var source = """
            using XmlSpecified;
            using System.Collections.Generic;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]  // Missing CollectionOptions for List
                public List<string> Items { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_NullableType_NoOption_DoesNotReportXSG005()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]  // Nullable int doesn't need explicit option
                public int? Value { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_ReferenceType_NoOption_DoesNotReportXSG005()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified]  // Complex reference type doesn't need explicit option
                public Address HomeAddress { get; set; }
            }

            public class Address
            {
                public string Street { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Theory]
    [InlineData("System.DayOfWeek")]
    [InlineData("System.DateTime")]
    [InlineData("MyStruct")]
    public void Generator_UnsupportedValueType_ReportsXSG006(string type)
    {
        // Arrange
        var source = $$"""
            using XmlSpecified;

            namespace TestNamespace;

            public struct MyStruct { }

            public partial class TestClass
            {
                [XmlSpecified]
                public {{type}} Value { get; set; }
            }
            """;

        // Act
        var (result, errors) = GeneratorTestHelper.RunAndCompile(source);

        // Assert
        Assert.Equal(["XSG006"], GeneratorTestHelper.GeneratorDiagnosticIds(result));
        Assert.Empty(errors);
        Assert.Empty(result.GeneratedTrees);
    }

    [Fact]
    public void Generator_InterfaceProperty_ReportsXSG007()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial interface IOrder
            {
                [XmlSpecified(NumericOptions.Positive)]
                int Quantity { get; set; }
            }
            """;

        // Act
        var (result, errors) = GeneratorTestHelper.RunAndCompile(source);

        // Assert
        Assert.Equal(["XSG007"], GeneratorTestHelper.GeneratorDiagnosticIds(result));
        Assert.Empty(errors);
    }
}
