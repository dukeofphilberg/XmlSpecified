using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct TypeInfo(
    string Name,
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
            Name = propertyType.ToDisplayString(),
            IsNullable = TypeUtilities.IsNullableValueType(propertyType),
            IsNumeric = TypeUtilities.IsNumericType(propertyType),
            IsReference = TypeUtilities.IsReferenceType(propertyType),
            IsString = TypeUtilities.IsStringType(propertyType),
            IsBool = TypeUtilities.IsBoolType(propertyType),
            IsArray = propertyType is IArrayTypeSymbol,
            IsCollection = TypeUtilities.IsCollectionType(propertyType),
            IsEnumerable = TypeUtilities.IsEnumerable(propertyType),
        };
    }
}
