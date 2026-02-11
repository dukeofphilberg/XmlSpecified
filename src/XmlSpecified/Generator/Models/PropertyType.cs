using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct PropertyType
{
    public bool IsNullable { get; init; }
    public bool IsNumeric { get; init; }
    public bool IsReference { get; init; }
    public bool IsString { get; init; }
    public bool IsBool { get; init; }
    public bool IsArray { get; init; }
    public bool IsCollection { get; init; }
    public bool IsEnumerable { get; init; }

    internal static PropertyType Create(ITypeSymbol propertyType)
    {
        return new PropertyType
        {
            IsNullable = TypeCheckResolver.IsNullableValueType(propertyType),
            IsNumeric = TypeCheckResolver.IsNumericType(propertyType),
            IsReference = TypeCheckResolver.IsReferenceType(propertyType),
            IsString = TypeCheckResolver.IsStringType(propertyType),
            IsBool = TypeCheckResolver.IsBoolType(propertyType),
            IsArray = propertyType is IArrayTypeSymbol,
            IsCollection = TypeCheckResolver.IsCollectionType(propertyType),
            IsEnumerable = TypeCheckResolver.IsEnumerable(propertyType),
        };
    }
}
