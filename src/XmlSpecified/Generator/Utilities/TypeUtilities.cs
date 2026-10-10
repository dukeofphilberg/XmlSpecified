using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Utilities;

/// <summary>
/// Resolves the appropriate type check expression for a property based on its type and options.
/// </summary>
internal static class TypeUtilities
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
    /// Determines if the given type is a collection type (List, ICollection, etc.),
    /// i.e. it exposes a <c>Count</c> property through ICollection&lt;T&gt; or ICollection.
    /// </summary>
    internal static bool IsCollectionType(ITypeSymbol type) =>
        ImplementsAny(type, SpecialType.System_Collections_Generic_ICollection_T)
        || ImplementsAny(type, "System.Collections.ICollection");

    /// <summary>
    /// Determines if the given type is a non-nullable value type that has no supported check
    /// (e.g. enums, DateTime, Guid, char or custom structs), which cannot be compared with null.
    /// </summary>
    internal static bool IsUnsupportedValueType(ITypeSymbol type) =>
        type.IsValueType && !IsNullableValueType(type) && !IsNumericType(type) && !IsBoolType(type);

    /// <summary>
    /// Determines if the given type implements IEnumerable&lt;T&gt;, so that <c>Enumerable.Any()</c> can be used.
    /// </summary>
    internal static bool IsEnumerable(ITypeSymbol type) =>
        ImplementsAny(type, SpecialType.System_Collections_Generic_IEnumerable_T);

    private static bool ImplementsAny(ITypeSymbol type, SpecialType specialType)
    {
        foreach (var iface in SelfAndAllInterfaces(type))
        {
            if (iface.OriginalDefinition.SpecialType == specialType)
            {
                return true;
            }
        }

        return false;
    }

    private static bool ImplementsAny(ITypeSymbol type, string fullName)
    {
        foreach (var iface in SelfAndAllInterfaces(type))
        {
            if (iface.OriginalDefinition.ToDisplayString() == fullName)
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<INamedTypeSymbol> SelfAndAllInterfaces(ITypeSymbol type)
    {
        if (type is not INamedTypeSymbol namedType)
        {
            yield break;
        }

        if (namedType.TypeKind == TypeKind.Interface)
        {
            yield return namedType;
        }

        foreach (var iface in namedType.AllInterfaces)
        {
            yield return iface;
        }
    }

    internal static string GetTypeKeyword(INamedTypeSymbol type)
    {
        if (type.IsRecord)
        {
            return type.TypeKind == TypeKind.Struct ? "record struct" : "record class";
        }

        return type.TypeKind == TypeKind.Struct ? "struct" : "class";
    }
}
