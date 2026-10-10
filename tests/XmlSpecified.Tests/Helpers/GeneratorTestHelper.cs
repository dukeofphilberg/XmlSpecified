using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using XmlSpecified.Generator;

namespace XmlSpecified.Tests.Helpers;

/// <summary>
/// Helper class for testing the source generator.
/// </summary>
internal static class GeneratorTestHelper
{
    /// <summary>
    /// All framework assemblies of the test runtime plus XmlSpecified, so test sources and
    /// generated code can use any BCL type. Built once, as loading the references is expensive.
    /// </summary>
    private static readonly Lazy<MetadataReference[]> References = new(() =>
        [
            .. ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
                .Split(Path.PathSeparator)
                .Select(path => MetadataReference.CreateFromFile(path)),
            MetadataReference.CreateFromFile(typeof(XmlSpecifiedAttribute).Assembly.Location),
        ]
    );

    /// <summary>
    /// Creates a compilation of the given source referencing the full framework.
    /// </summary>
    internal static CSharpCompilation CreateCompilation(string source) =>
        CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [CSharpSyntaxTree.ParseText(source)],
            references: References.Value,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

    /// <summary>
    /// Runs the generator and returns the generator result plus the errors of the
    /// compilation including the generated sources.
    /// </summary>
    internal static (GeneratorDriverRunResult Result, Diagnostic[] CompilationErrors) RunAndCompile(
        string source
    )
    {
        var result = CSharpGeneratorDriver
            .Create(new XmlSpecifiedGenerator())
            .RunGeneratorsAndUpdateCompilation(
                CreateCompilation(source),
                out var outputCompilation,
                out _
            )
            .GetRunResult();

        var errors = outputCompilation
            .GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ToArray();

        return (result, errors);
    }

    /// <summary>
    /// Snapshots the generated sources and generator diagnostics, after asserting
    /// the generated code compiles.
    /// </summary>
    internal static Task Verify(string source)
    {
        var (result, errors) = RunAndCompile(source);

        Assert.Empty(errors);

        return Verifier.Verify(result);
    }
}
