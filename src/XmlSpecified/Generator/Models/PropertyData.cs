using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Models;

/// <summary>
/// Data about a property decorated with XmlSpecified.
/// </summary>
internal readonly record struct PropertyData(
    string PropertyName,
    TypeInfo PropertyType,
    string PropertyTypeDisplayString,
    SpecifiedOptions AttributeValues
)
{
    internal static PropertyData Create(
        IPropertySymbol symbol,
        TypeInfo typeInfo,
        SpecifiedOptions attributeValues
    )
    {
        return new PropertyData
        {
            AttributeValues = attributeValues,
            PropertyName = symbol.Name,
            PropertyType = typeInfo,
            PropertyTypeDisplayString = symbol.Type.ToDisplayString(),
        };
    }
}
