using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct DiagnosticData(
    PropertyLocation Location,
    string ContainingTypeName,
    bool HasSetter,
    bool IsStatic,
    bool IsPartial,
    bool HasSpecifiedPropertyExists,
    RequiredAttributeOption RequiredOption
)
{
    internal static DiagnosticData Create(
        IPropertySymbol symbol,
        RequiredAttributeOption requiredOption
    )
    {
        return new DiagnosticData
        {
            Location = PropertyLocation.Create(symbol),
            ContainingTypeName = symbol.ContainingType.Name,
            HasSetter = symbol.SetMethod != null,
            IsStatic = symbol.IsStatic,
            IsPartial = symbol.IsContainingClassPartial(),
            HasSpecifiedPropertyExists = symbol.SpecifiedPropertyExists(),
            RequiredOption = requiredOption,
        };
    }
}
