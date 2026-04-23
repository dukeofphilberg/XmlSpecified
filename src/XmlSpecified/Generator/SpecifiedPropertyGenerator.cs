using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XmlSpecified.Generator.Diagnostics;
using XmlSpecified.Generator.Models;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator;

/// <summary>
/// Incremental source generator that creates [PropertyName]Specified properties
/// for properties decorated with [XmlSpecified].
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class SpecifiedPropertyGenerator : IIncrementalGenerator
{
    private const string AttributeFullName = "XmlSpecified.XmlSpecifiedAttribute";
    private const string Version = "0.1.0";

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Find all properties with the XmlSpecified attribute
        var allProperties = context.SyntaxProvider.ForAttributeWithMetadataName(
            AttributeFullName,
            predicate: (node, _) => node is BasePropertyDeclarationSyntax,
            transform: (ctx, _) => PropertyTarget.Create(ctx)
        );

        // Group properties by containing PropertyContainer
        var grouped = allProperties
            .Collect()
            .WithTrackingName("Group by namespace")
            .SelectMany(
                static (props, _) =>
                    props
                        .GroupBy(p => p.PropertyContainer)
                        .Select(g => new PropertyGrouping
                        {
                            Container = g.Key,
                            Properties = new EquatableArray<PropertyTarget>(
                                g.Select(p => p).ToArray()
                            ),
                        })
            );

        context.RegisterImplementationSourceOutput(
            grouped,
            static (spc, source) => Execute(spc, source)
        );
    }

    /// <summary>
    /// Executes the generator logic.
    /// </summary>
    private static void Execute(SourceProductionContext context, PropertyGrouping propertyGrouping)
    {
        if (propertyGrouping.Properties.Count == 0)
        {
            return;
        }

        var (propertyContainer, properties) = (
            propertyGrouping.Container,
            propertyGrouping.Properties
        );

        var hasCode = false;

        try
        {
            var builder = new SpecifiedCodeBuilder(propertyContainer, Version);

            foreach (var property in properties)
            {
                var (container, propertyData, diagnosticData) = property;
                var analysisResult = PropertyAnalyzer.Analyze(
                    propertyContainer,
                    propertyData,
                    diagnosticData
                );

                // Report diagnostics
                foreach (var diagnostic in analysisResult.Diagnostics)
                {
                    context.ReportDiagnostic(diagnostic);
                }

                // Add property if we should generate
                if (analysisResult.ShouldGenerate)
                {
                    builder.AddProperty(propertyData);
                    hasCode = true;
                }
            }

            // Generate the source file
            if (hasCode)
            {
                var sourceCode = builder.Build();
                var fileName = Utilities.Utilities.GetFileName(propertyContainer);
                context.AddSource(fileName, sourceCode);
            }
        }
        catch (Exception ex)
        {
            var diagnostic = DiagnosticReporter.ReportGeneratorError(ex);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
