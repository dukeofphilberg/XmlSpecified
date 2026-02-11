namespace XmlSpecifiedGenerator.Tests.GeneratorTests;

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

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        return Verify(diagnostics);
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

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        return Verify(diagnostics);
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

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        return Verify(diagnostics);
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
                [XmlSpecified(NumericOptions = NumericOptions.Positive, StringOptions = StringOptions.NonEmpty)]
                public string Name { get; set; }  // Both options specified
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        return Verify(diagnostics);
    }

    [Fact]
    public Task Generator_MissingRequiredOption_NonNullableInt_ReportsXSG006()
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

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        return Verify(diagnostics);
    }

    [Fact]
    public Task Generator_MissingRequiredOption_String_ReportsXSG006()
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

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        return Verify(diagnostics);
    }

    [Fact]
    public Task Generator_MissingRequiredOption_Bool_ReportsXSG006()
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

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        return Verify(diagnostics);
    }

    [Fact]
    public Task Generator_MissingRequiredOption_Collection_ReportsXSG006()
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

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        return Verify(diagnostics);
    }

    [Fact]
    public Task Generator_NullableType_NoOption_DoesNotReportXSG006()
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

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        return Verify(diagnostics);
    }

    [Fact]
    public Task Generator_ReferenceType_NoOption_DoesNotReportXSG006()
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

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        return Verify(diagnostics);
    }
}
