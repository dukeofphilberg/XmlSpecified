using System.Linq;
using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Models;

/// <summary>
/// Result of analyzing a property.
/// </summary>
internal readonly record struct PropertyAnalysisResult(Diagnostic[] Diagnostics)
{
    /// <summary>
    /// Whether or not the generator should generate the Specified property for the property.
    /// </summary>
    public bool ShouldGenerate => !Diagnostics.Any(x => x.IsWarningAsError);
}
