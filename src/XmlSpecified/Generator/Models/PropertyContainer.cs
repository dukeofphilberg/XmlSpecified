using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct PropertyContainer
{
    public string Namespace { get; init; }
    public EquatableArray<PropertyClass> PropertyClasses { get; init; }

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
