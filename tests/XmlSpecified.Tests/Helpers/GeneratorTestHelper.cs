using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using XmlSpecified.Generator;

namespace XmlSpecified.Tests.Helpers;

/// <summary>
/// Helper class for testing the source generator.
/// </summary>
internal static class GeneratorTestHelper
{
    internal static GeneratorDriver GetDriver(string source)
    {
        // Parse the provided string into a C# syntax tree
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

        // Get all assemblies needed for a working compilation
        var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(XmlSpecifiedAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Collections.dll")),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "netstandard.dll")),
            MetadataReference.CreateFromFile(
                Path.Combine(assemblyPath, "System.ComponentModel.dll")
            ),
        };

        // Create a Roslyn compilation for the syntax tree.
        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [syntaxTree],
            references: references
        );

        // Create an instance of our source generator
        var generator = new XmlSpecifiedGenerator();

        // The GeneratorDriver is used to run our generator against a compilation
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the source generator
        return driver.RunGenerators(compilation);
    }

    internal static Task Verify(string source)
    {
        var driver = GetDriver(source);

        return Verifier.Verify(driver);
    }
}
