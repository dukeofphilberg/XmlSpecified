using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct DiagnosticData
{
    public PropertyLocation Location { get; init; }
    public bool HasSetter { get; init; }
    public bool IsStatic { get; init; }
    public bool IsPartial { get; init; }
    public bool HasSpecifiedPropertyExists { get; init; }
    public string? RequiredOptionName { get; init; }

    internal static DiagnosticData Create(IPropertySymbol symbol)
    {
        return new DiagnosticData
        {
            Location = PropertyLocation.Create(symbol),
            HasSetter = symbol.SetMethod != null,
            IsStatic = symbol.IsStatic,
            IsPartial = IsContainingClassPartial(symbol),
            HasSpecifiedPropertyExists = SpecifiedPropertyExists(symbol),
            RequiredOptionName = GetRequiredOptionName(symbol),
        };
    }

    /// <summary>
    /// Checks if the containing class is partial.
    /// </summary>
    public static bool IsContainingClassPartial(IPropertySymbol symbol)
    {
        var containingType = symbol.ContainingType;

        // Check all declarations of the type
        foreach (var syntaxRef in containingType.DeclaringSyntaxReferences)
        {
            if (syntaxRef.GetSyntax() is TypeDeclarationSyntax typeDecl)
            {
                foreach (var modifier in typeDecl.Modifiers)
                {
                    if (modifier.Text == "partial")
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if a Specified property already exists for the given property name.
    /// </summary>
    public static bool SpecifiedPropertyExists(IPropertySymbol symbol)
    {
        var specifiedName = $"{symbol.Name}Specified";
        var containingType = symbol.ContainingType;

        foreach (var member in containingType.GetMembers())
        {
            if (member is IPropertySymbol prop && prop.Name == specifiedName)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines the required option type for a property type.
    /// Returns null if no explicit option is required (nullable value types, reference types).
    /// </summary>
    public static string? GetRequiredOptionName(IPropertySymbol symbol)
    {
        var propertyType = symbol.Type;

        // Nullable value types don't require explicit options
        if (TypeCheckResolver.IsNullableValueType(propertyType))
        {
            return null;
        }

        // Non-nullable value types require explicit options
        if (TypeCheckResolver.IsBoolType(propertyType))
        {
            return "BoolOptions";
        }

        if (TypeCheckResolver.IsNumericType(propertyType))
        {
            return "NumericOptions";
        }

        // String requires StringOptions
        if (TypeCheckResolver.IsStringType(propertyType))
        {
            return "StringOptions";
        }

        // Collections require CollectionOptions
        if (
            propertyType.TypeKind == TypeKind.Array
            || TypeCheckResolver.IsCollectionType(propertyType)
        )
        {
            return "CollectionOptions";
        }

        // Other reference types have default behavior (null check)
        return null;
    }
}
