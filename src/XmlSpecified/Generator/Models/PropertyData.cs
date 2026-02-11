using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Models;

/// <summary>
/// Data about a property decorated with XmlSpecified.
/// </summary>
internal readonly record struct PropertyData
{
    public string PropertyName { get; init; }
    public PropertyType PropertyType { get; init; }
    public string PropertyTypeDisplayString { get; init; }
    public AttributeValues AttributeValues { get; init; }

    internal static PropertyData Create(IPropertySymbol symbol)
    {
        var attributes = symbol.GetAttributes();
        var attributeValues = AttributeValues.Create(attributes[0]);

        return new PropertyData
        {
            AttributeValues = attributeValues,
            PropertyName = symbol.Name,
            PropertyType = PropertyType.Create(symbol.Type),
            PropertyTypeDisplayString = symbol.Type.ToDisplayString(),
        };
    }
}
