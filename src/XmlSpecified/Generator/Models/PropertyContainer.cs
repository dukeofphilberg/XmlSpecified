using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct PropertyContainer(
    string Namespace,
    EquatableArray<PropertyClass> PropertyClasses
)
{
    internal static PropertyContainer Create(IPropertySymbol symbol)
    {
        return new PropertyContainer
        {
            Namespace =
                symbol.ContainingNamespace.ToDisplayString(
                    SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(
                        SymbolDisplayGlobalNamespaceStyle.Omitted
                    )
                ) ?? string.Empty,
            PropertyClasses = PropertyClass.Construct(symbol),
        };
    }
}
