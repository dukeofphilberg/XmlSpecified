using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct DiagnosticData(
    PropertyLocation Location,
    bool HasSetter,
    bool IsStatic,
    bool IsPartial,
    bool HasSpecifiedPropertyExists,
    RequiredOption RequiredOption
)
{
    internal static DiagnosticData Create(
        IPropertySymbol symbol,
        TypeInfo typeInfo,
        SpecifiedOptions attributeValues
    )
    {
        return new DiagnosticData
        {
            Location = PropertyLocation.Create(symbol),
            HasSetter = symbol.SetMethod != null,
            IsStatic = symbol.IsStatic,
            IsPartial = symbol.IsContainingClassPartial(),
            HasSpecifiedPropertyExists = symbol.SpecifiedPropertyExists(),
            RequiredOption = RequiredOption.Create(typeInfo, attributeValues),
        };
    }
}
