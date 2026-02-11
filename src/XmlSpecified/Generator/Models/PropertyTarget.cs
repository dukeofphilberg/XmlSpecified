using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Models;

internal readonly record struct PropertyTarget
{
    public PropertyContainer PropertyContainer { get; init; }
    public PropertyData PropertyData { get; init; }
    public DiagnosticData DiagnosticData { get; init; }

    internal static PropertyTarget Create(GeneratorAttributeSyntaxContext context)
    {
        var symbol = (IPropertySymbol)context.TargetSymbol;

        return new PropertyTarget
        {
            PropertyContainer = PropertyContainer.Create(symbol),
            PropertyData = PropertyData.Create(symbol),
            DiagnosticData = DiagnosticData.Create(symbol),
        };
    }
}
