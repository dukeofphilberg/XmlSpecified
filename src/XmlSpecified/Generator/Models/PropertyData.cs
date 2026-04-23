using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Models;

/// <summary>
/// Data about a property decorated with XmlSpecified.
/// </summary>
internal readonly record struct PropertyData(
    string PropertyName,
    TypeInfo PropertyType,
    AttributeOptions AttributeOptions
)
{
    internal static PropertyData Create(
        IPropertySymbol symbol,
        TypeInfo typeInfo,
        AttributeOptions attributeOptions
    )
    {
        return new PropertyData
        {
            AttributeOptions = attributeOptions,
            PropertyName = symbol.Name,
            PropertyType = typeInfo,
        };
    }
}
