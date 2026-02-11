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
    /// Result of analyzing a property.
    /// </summary>
    public sealed class AnalysisResult
    {
        public bool ShouldGenerate { get; }
        public Diagnostic[] Diagnostics { get; }

        public AnalysisResult(bool shouldGenerate, Diagnostic[] diagnostics)
        {
            ShouldGenerate = shouldGenerate;
            Diagnostics = diagnostics;
        }
    }

    /// <summary>
    /// Gets a list of specified option type names.
    /// </summary>
    public static string GetSpecifiedOptionNames(AttributeValues values)
    {
        var names = new List<string>();
        if (values.NumericOptions is not null)
            names.Add("NumericOptions");
        if (values.StringOptions is not null)
            names.Add("StringOptions");
        if (values.BoolOptions is not null)
            names.Add("BoolOptions");
        if (values.CollectionOptions is not null)
            names.Add("CollectionOptions");
        return string.Join(", ", names);
    }

    /// <summary>
    /// Analyzes a property and generates the check expression.
    /// </summary>
    public static AnalysisResult Analyze(
        PropertyContainer propertyContainer,
        PropertyData propertyData,
        DiagnosticData diagnosticData
    )
    {
        var className = string.Empty;
        foreach (var parentClass in propertyContainer.PropertyClasses)
        {
            className = parentClass.Name;
        }

        var diagnostics = new List<Diagnostic>();
        var shouldGenerate = true;
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
            shouldGenerate = false;
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
            shouldGenerate = false;
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
            shouldGenerate = false;
        }

        // XSG005: Missing required option
        var hasRequiredOption = diagnosticData.RequiredOptionName switch
        {
            "NumericOptions" => propertyData.AttributeValues.NumericOptions is not null,
            "StringOptions" => propertyData.AttributeValues.StringOptions is not null,
            "BoolOptions" => propertyData.AttributeValues.BoolOptions is not null,
            "CollectionOptions" => propertyData.AttributeValues.CollectionOptions is not null,
            _ => true,
        };

        if (!hasRequiredOption)
        {
            diagnostics.Add(
                DiagnosticReporter.ReportMissingRequiredOption(
                    location,
                    propertyData.PropertyName,
                    propertyData.PropertyTypeDisplayString,
                    diagnosticData.RequiredOptionName ?? string.Empty
                )
            );
            shouldGenerate = false;
        }

        var result = new AnalysisResult(shouldGenerate, diagnostics.ToArray());

        return result;
    }
}
