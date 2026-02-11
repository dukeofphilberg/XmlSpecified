using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using XmlSpecified.Generator;

namespace XmlSpecifiedGenerator.Tests.Helpers;

/// <summary>
/// Helper class for testing the source generator.
/// </summary>
public static class GeneratorTestHelper
{
    /// <summary>
    /// Runs the XmlSpecifiedGenerator on the provided source code.
    /// </summary>
    /// <param name="source">The source code to analyze.</param>
    /// <returns>A tuple containing the generated output and any diagnostics.</returns>
    public static (
        ImmutableArray<GeneratedSourceResult> Generated,
        ImmutableArray<Diagnostic> Diagnostics
    ) RunGenerator(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);

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

        var compilation = CSharpCompilation.Create(
            assemblyName: "TestAssembly",
            syntaxTrees: [syntaxTree],
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        var generator = new SpecifiedPropertyGenerator();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out var outputCompilation,
            out var diagnostics
        );

        var runResult = driver.GetRunResult();
        var generatedSources = runResult.Results[0].GeneratedSources;

        return (generatedSources, diagnostics);
    }

    /// <summary>
    /// Gets the generated source code for the first generated file, if any.
    /// </summary>
    public static string? GetGeneratedSource(string source)
    {
        var (generated, _) = RunGenerator(source);

        if (generated.Length == 0)
        {
            return null;
        }

        return generated[0].SourceText.ToString();
    }

    /// <summary>
    /// Gets all diagnostics from running the generator.
    /// </summary>
    public static ImmutableArray<Diagnostic> GetDiagnostics(string source)
    {
        var (_, diagnostics) = RunGenerator(source);
        return diagnostics;
    }

    public static Task Verify(string source)
    {
        // Parse the provided string into a C# syntax tree
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

        // Create references for assemblies we require
        // We could add multiple references if required
        //IEnumerable<PortableExecutableReference> references = new[]
        //{
        //    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        //};

        // Get all assemblies needed for a working compilation
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(XmlSpecifiedAttribute).Assembly.Location),
        };

        // Add runtime assemblies
        var assemblyPath = System.IO.Path.GetDirectoryName(typeof(object).Assembly.Location)!;
        references.Add(
            MetadataReference.CreateFromFile(
                System.IO.Path.Combine(assemblyPath, "System.Runtime.dll")
            )
        );
        references.Add(
            MetadataReference.CreateFromFile(
                System.IO.Path.Combine(assemblyPath, "System.Collections.dll")
            )
        );

        // Add netstandard reference (needed because generator dll is netstandard2.0)
        var netstandardPath = System.IO.Path.Combine(assemblyPath, "netstandard.dll");
        if (System.IO.File.Exists(netstandardPath))
        {
            references.Add(MetadataReference.CreateFromFile(netstandardPath));
        }

        // Add System.ComponentModel (for GeneratedCodeAttribute)
        var componentModelPath = System.IO.Path.Combine(assemblyPath, "System.ComponentModel.dll");
        if (System.IO.File.Exists(componentModelPath))
        {
            references.Add(MetadataReference.CreateFromFile(componentModelPath));
        }

        // Create a Roslyn compilation for the syntax tree.
        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [syntaxTree],
            references: references
        );

        // Create an instance of our source generator
        var generator = new SpecifiedPropertyGenerator();

        // The GeneratorDriver is used to run our generator against a compilation
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the source generator!
        driver = driver.RunGenerators(compilation);

        // Use verify to snapshot test the source generator output!
        return Verifier.Verify(driver);
    }
}
