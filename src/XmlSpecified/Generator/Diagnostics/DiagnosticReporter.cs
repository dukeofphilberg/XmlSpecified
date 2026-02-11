using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Diagnostics;

/// <summary>
/// Helper class for reporting diagnostics during source generation.
/// </summary>
internal static class DiagnosticReporter
{
    /// <summary>
    /// Creates a diagnostic for a non-partial class.
    /// </summary>
    public static Diagnostic ReportNonPartialClass(
        Location location,
        string propertyName,
        string className
    )
    {
        return Diagnostic.Create(
            DiagnosticDescriptors.NonPartialClass,
            location,
            propertyName,
            className
        );
    }

    /// <summary>
    /// Creates a diagnostic for a duplicate Specified property.
    /// </summary>
    public static Diagnostic ReportDuplicateSpecifiedProperty(
        Location location,
        string propertyName,
        string className
    )
    {
        return Diagnostic.Create(
            DiagnosticDescriptors.DuplicateSpecifiedProperty,
            location,
            propertyName,
            className
        );
    }

    /// <summary>
    /// Creates a diagnostic for a read-only property.
    /// </summary>
    public static Diagnostic ReportReadOnlyProperty(Location location, string propertyName)
    {
        return Diagnostic.Create(DiagnosticDescriptors.ReadOnlyProperty, location, propertyName);
    }

    /// <summary>
    /// Creates a diagnostic for a static property.
    /// </summary>
    public static Diagnostic ReportStaticProperty(Location location, string propertyName)
    {
        return Diagnostic.Create(DiagnosticDescriptors.StaticProperty, location, propertyName);
    }

    /// <summary>
    /// Creates a diagnostic for a missing required option.
    /// </summary>
    public static Diagnostic ReportMissingRequiredOption(
        Location location,
        string propertyName,
        string typeName,
        string requiredOption
    )
    {
        return Diagnostic.Create(
            DiagnosticDescriptors.MissingRequiredOption,
            location,
            propertyName,
            typeName,
            requiredOption
        );
    }
}
