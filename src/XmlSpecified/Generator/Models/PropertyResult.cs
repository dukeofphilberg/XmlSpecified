namespace XmlSpecified.Generator.Models;

/// <summary>
/// Represents the result of analyzing a property, containing the property data
/// and any associated diagnostic information.
/// </summary>
internal readonly record struct PropertyResult(PropertyData Property, DiagnosticData Diagnostic);
