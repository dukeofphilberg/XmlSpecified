namespace XmlSpecified.Tests.GeneratorTests;

public class CollectionOptionsTests
{
    [Fact]
    public Task Generator_CollectionOptions_NonEmpty_Array_GeneratesLengthCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(CollectionOptions.NonEmpty)]
                public int[] Numbers { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CollectionOptions_NonNull_Array_GeneratesNullCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(CollectionOptions.NonNull)]
                public string[] Tags { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CollectionOptions_NonEmpty_List_GeneratesCountCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;
            using System.Collections.Generic;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(CollectionOptions.NonEmpty)]
                public List<string> Items { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CollectionOptions_NonNull_List_GeneratesNullCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;
            using System.Collections.Generic;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(CollectionOptions.NonNull)]
                public List<int> Values { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CollectionOptions_NonEmpty_IList_GeneratesCountCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;
            using System.Collections.Generic;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(CollectionOptions.NonEmpty)]
                public IList<string> Items { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CollectionOptions_NonNull_IList_GeneratesNullCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;
            using System.Collections.Generic;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(CollectionOptions.NonNull)]
                public IList<string> Items { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CollectionOptions_NonEmpty_Enumerable_GeneratesAnyCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;
            using System.Collections.Generic;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(CollectionOptions.NonEmpty)]
                public IEnumerable<string> Items { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }

    [Fact]
    public Task Generator_CollectionOptions_NonNull_Enumerable_GeneratesNullCheck()
    {
        // Arrange
        var source = """
            using XmlSpecified;
            using System.Collections.Generic;

            namespace TestNamespace;

            public partial class TestClass
            {
                [XmlSpecified(CollectionOptions.NonNull)]
                public IEnumerable<string> Items { get; set; }
            }
            """;

        // Act - Assert
        return GeneratorTestHelper.Verify(source);
    }
}
