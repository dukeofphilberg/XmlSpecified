using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct TypeInfo(
    bool IsNullable,
    bool IsNumeric,
    bool IsReference,
    bool IsString,
    bool IsBool,
    bool IsArray,
    bool IsCollection,
    bool IsEnumerable
)
{
    internal static TypeInfo Create(ITypeSymbol propertyType)
    {
        return new TypeInfo
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
