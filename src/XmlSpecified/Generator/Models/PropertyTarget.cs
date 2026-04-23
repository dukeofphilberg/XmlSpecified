using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Models;

internal readonly record struct PropertyTarget(
    PropertyContainer PropertyContainer,
    PropertyData PropertyData,
    DiagnosticData DiagnosticData
)
{
    internal static PropertyTarget Create(GeneratorAttributeSyntaxContext context)
    {
        var symbol = (IPropertySymbol)context.TargetSymbol;
        var attributes = symbol.GetAttributes();
        var attributeValues = SpecifiedOptions.Create(attributes[0]);
        var typeInfo = TypeInfo.Create(symbol.Type);

        return new PropertyTarget
        {
            PropertyContainer = PropertyContainer.Create(symbol),
            PropertyData = PropertyData.Create(symbol, typeInfo, attributeValues),
            DiagnosticData = DiagnosticData.Create(symbol, typeInfo, attributeValues),
        };
    }
}
