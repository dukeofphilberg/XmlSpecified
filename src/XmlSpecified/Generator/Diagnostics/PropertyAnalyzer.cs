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
        PropertyContainer propertyContainer,
        PropertyData propertyData,
        DiagnosticData diagnosticData
    )
    {
        var last = propertyContainer.PropertyClasses.Count - 1;
        var className = propertyContainer.PropertyClasses.AsSpan()[last].Name;
        var diagnostics = new List<Diagnostic>();
        var location = diagnosticData.Location.GetLocation();

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

        // XSG002: Duplicate Specified property
        if (diagnosticData.HasSpecifiedPropertyExists)
        {
            diagnostics.Add(
                DiagnosticReporter.ReportDuplicateSpecifiedProperty(
                    location,
                    propertyData.PropertyName,
                    className
                )
            );
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
        if (!diagnosticData.RequiredOption.IsRequired)
        {
            diagnostics.Add(
                DiagnosticReporter.ReportMissingRequiredOption(
                    location,
                    propertyData.PropertyName,
                    propertyData.PropertyTypeDisplayString,
                    diagnosticData.RequiredOption.OptionName
                )
            );
        }

        return new PropertyAnalysisResult { Diagnostics = [.. diagnostics] };
    }
}
