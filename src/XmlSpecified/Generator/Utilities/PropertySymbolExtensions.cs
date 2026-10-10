using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XmlSpecified.Generator.Models;

namespace XmlSpecified.Generator.Utilities;

internal static class PropertySymbolExtensions
{
    /// <summary>
    /// Checks the containing type and every type it is nested in for being partial and
    /// being a class or struct, stopping at the first type that fails either check.
    /// </summary>
    internal static ContainingTypeInfo AnalyzeContainingTypes(this IPropertySymbol symbol)
    {
        var type = symbol.ContainingType;

        while (type is not null)
        {
            var isPartial = IsPartial(type);
            var isSupported = type.TypeKind is TypeKind.Class or TypeKind.Struct;

            if (!isPartial || !isSupported)
            {
                return new ContainingTypeInfo(isPartial, isSupported);
            }

            type = type.ContainingType;
        }

        return new ContainingTypeInfo(IsPartial: true, IsSupported: true);
    }

    private static bool IsPartial(INamedTypeSymbol type)
    {
        // Check all declarations of the type
        foreach (var syntaxRef in type.DeclaringSyntaxReferences)
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
    internal static bool SpecifiedPropertyExists(this IPropertySymbol symbol)
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
}
