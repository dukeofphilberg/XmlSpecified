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
        var attributeOptions = AttributeOptions.Create(attributes[0]);
        var typeInfo = TypeInfo.Create(symbol.Type);
        var requiredOption = RequiredAttributeOption.Create(typeInfo, attributeOptions);

        return new PropertyTarget
        {
            PropertyContainer = PropertyContainer.Create(symbol),
            PropertyData = PropertyData.Create(symbol, typeInfo, attributeOptions),
            DiagnosticData = DiagnosticData.Create(symbol, requiredOption),
        };
    }
}
