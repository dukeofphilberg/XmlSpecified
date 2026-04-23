using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace XmlSpecified.Generator.Utilities;

internal static class PropertySymbolExtensions
{
    /// <summary>
    /// Checks if the containing class is partial.
    /// </summary>
    internal static bool IsContainingClassPartial(this IPropertySymbol symbol)
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
