using System;
using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Diagnostics;

/// <summary>
/// Helper class for reporting diagnostics during source generation.
/// </summary>
internal static class DiagnosticReporter
{
    /// <summary>
    /// Creates a diagnostic for a generator error, including the exception details.
    /// </summary>
    internal static Diagnostic ReportGeneratorError(Exception ex) =>
        Diagnostic.Create(DiagnosticDescriptors.GeneratorError, Location.None, ex.ToString());

    /// <summary>
    /// Creates a diagnostic for a non-partial class.
    /// </summary>
    internal static Diagnostic ReportNonPartialClass(
        Location location,
        string propertyName,
        string className
    ) =>
        Diagnostic.Create(DiagnosticDescriptors.NonPartialClass, location, propertyName, className);

    /// <summary>
    /// Creates a diagnostic for a duplicate Specified property.
    /// </summary>
    internal static Diagnostic ReportDuplicateSpecifiedProperty(
        Location location,
        string propertyName,
        string className
    ) =>
        Diagnostic.Create(
            DiagnosticDescriptors.DuplicateSpecifiedProperty,
            location,
            propertyName,
            className
        );

    /// <summary>
    /// Creates a diagnostic for a read-only property.
    /// </summary>
    internal static Diagnostic ReportReadOnlyProperty(Location location, string propertyName) =>
        Diagnostic.Create(DiagnosticDescriptors.ReadOnlyProperty, location, propertyName);

    /// <summary>
    /// Creates a diagnostic for a static property.
    /// </summary>
    internal static Diagnostic ReportStaticProperty(Location location, string propertyName) =>
        Diagnostic.Create(DiagnosticDescriptors.StaticProperty, location, propertyName);

    /// <summary>
    /// Creates a diagnostic for a missing required attribute option.
    /// </summary>
    internal static Diagnostic ReportMissingAttributeOption(
        Location location,
        string propertyName,
        string typeName,
        string attributeOptionName
    ) =>
        Diagnostic.Create(
            DiagnosticDescriptors.MissingRequiredAttributeOption,
            location,
            propertyName,
            typeName,
            attributeOptionName
        );
}
