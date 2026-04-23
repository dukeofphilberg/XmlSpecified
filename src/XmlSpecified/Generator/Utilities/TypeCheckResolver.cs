using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Utilities;

/// <summary>
/// Resolves the appropriate type check expression for a property based on its type and options.
/// </summary>
internal static class TypeCheckResolver
{
    /// <summary>
    /// Determines if the given type is a nullable value type (e.g., int?, DateTime?).
    /// </summary>
    internal static bool IsNullableValueType(ITypeSymbol type) =>
        type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;

    /// <summary>
    /// Determines if the given type is a numeric type.
    /// </summary>
    internal static bool IsNumericType(ITypeSymbol type)
    {
        return type.SpecialType
            is SpecialType.System_Byte
                or SpecialType.System_SByte
                or SpecialType.System_Int16
                or SpecialType.System_UInt16
                or SpecialType.System_Int32
                or SpecialType.System_UInt32
                or SpecialType.System_Int64
                or SpecialType.System_UInt64
                or SpecialType.System_Single
                or SpecialType.System_Double
                or SpecialType.System_Decimal;
    }

    /// <summary>
    /// Determines if the given type is a string.
    /// </summary>
    internal static bool IsStringType(ITypeSymbol type) =>
        type.SpecialType == SpecialType.System_String;

    /// <summary>
    /// Determines if the given type is a boolean.
    /// </summary>
    internal static bool IsBoolType(ITypeSymbol type) =>
        type.SpecialType == SpecialType.System_Boolean;

    /// <summary>
    /// Determines if the given type is a collection type (List, ICollection, etc.).
    /// </summary>
    internal static bool IsCollectionType(ITypeSymbol type)
    {
        // Check for common collection interfaces
        if (type is INamedTypeSymbol namedType)
        {
            // Check if it implements ICollection<T> or ICollection
            foreach (var iface in namedType.AllInterfaces)
            {
                var name = iface.OriginalDefinition.ToDisplayString();
                if (
                    name
                    is "System.Collections.Generic.ICollection"
                        or "System.Collections.Generic.ICollection<T>"
                )
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Determines if the given type is a reference type (excluding string and collections).
    /// </summary>
    internal static bool IsReferenceType(ITypeSymbol type)
    {
        return type.IsReferenceType && !IsStringType(type) && !IsCollectionType(type);
    }

    internal static bool IsEnumerable(ITypeSymbol propertyType)
    {
        if (propertyType is INamedTypeSymbol namedType)
        {
            foreach (var iface in namedType.AllInterfaces)
            {
                if (iface.OriginalDefinition.ToDisplayString() == "System.Collections.IEnumerable")
                {
                    return true;
                }
            }
        }
        return false;
    }
}
