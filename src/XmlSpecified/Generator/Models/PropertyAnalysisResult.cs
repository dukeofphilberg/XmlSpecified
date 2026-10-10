using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Models;

/// <summary>
/// Result of analyzing a property.
/// </summary>
/// <param name="Diagnostics">The diagnostics to report for the property.</param>
/// <param name="ShouldGenerate">Whether the generator should generate the Specified property.</param>
internal readonly record struct PropertyAnalysisResult(
    Diagnostic[] Diagnostics,
    bool ShouldGenerate
);
