using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Models;

namespace XmlSpecified.Generator.Diagnostics;

/// <summary>
/// Analyzes properties decorated with [XmlSpecified] attribute.
/// </summary>
internal static class PropertyAnalyzer
{
    /// <summary>
    /// Analyzes a property and generates the check expression.
    /// </summary>
    internal static PropertyAnalysisResult Analyze(
        PropertyData propertyData,
        DiagnosticData diagnosticData
    )
    {
        var className = diagnosticData.ContainingTypeName;
        var diagnostics = new List<Diagnostic>();
        var location = diagnosticData.Location.GetLocation();

        // XSG007: Interface (or otherwise unsupported) containing type
        if (!diagnosticData.IsContainingTypeSupported)
        {
            diagnostics.Add(
                DiagnosticReporter.ReportUnsupportedContainingType(
                    location,
                    propertyData.PropertyName,
                    className
                )
            );

            // The remaining checks don't apply when there is no class to generate into
            return new PropertyAnalysisResult([.. diagnostics], ShouldGenerate: false);
        }

        // XSG001: Non-partial class
        if (!diagnosticData.IsPartial)
        {
            diagnostics.Add(
                DiagnosticReporter.ReportNonPartialClass(
                    location,
                    propertyData.PropertyName,
                    className
                )
            );
        }

        // XSG002: A member with the Specified name already exists.
        // Only a warning, as the user's own member works, but there is nothing to generate.
        if (diagnosticData.HasSpecifiedMember)
        {
            diagnostics.Add(
                DiagnosticReporter.ReportDuplicateSpecifiedProperty(
                    location,
                    propertyData.PropertyName,
                    className
                )
            );

            return new PropertyAnalysisResult([.. diagnostics], ShouldGenerate: false);
        }

        // XSG003: Read-only property
        if (!diagnosticData.HasSetter)
        {
            diagnostics.Add(
                DiagnosticReporter.ReportReadOnlyProperty(location, propertyData.PropertyName)
            );
            // Still generate, just warn
        }

        // XSG004: Static property
        if (diagnosticData.IsStatic)
        {
            diagnostics.Add(
                DiagnosticReporter.ReportStaticProperty(location, propertyData.PropertyName)
            );
        }

        // XSG005: Missing required option
        if (!diagnosticData.RequiredOption.IsPresent)
        {
            diagnostics.Add(
                DiagnosticReporter.ReportMissingAttributeOption(
                    location,
                    propertyData.PropertyName,
                    propertyData.PropertyType.Name,
                    diagnosticData.RequiredOption.RequiredOptionsName
                )
            );
        }

        // XSG006: Unsupported non-nullable value type (enum, DateTime, struct, ...)
        if (propertyData.PropertyType.IsUnsupportedValueType)
        {
            diagnostics.Add(
                DiagnosticReporter.ReportUnsupportedPropertyType(
                    location,
                    propertyData.PropertyName,
                    propertyData.PropertyType.Name
                )
            );
        }

        var hasError = diagnostics.Exists(d => d.Severity == DiagnosticSeverity.Error);

        return new PropertyAnalysisResult([.. diagnostics], ShouldGenerate: !hasError);
    }
}
