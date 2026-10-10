using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct DiagnosticData(
    PropertyLocation Location,
    string ContainingTypeName,
    bool IsContainingTypeSupported,
    bool HasSetter,
    bool IsStatic,
    bool IsPartial,
    bool HasSpecifiedMember,
    RequiredAttributeOption RequiredOption
)
{
    internal static DiagnosticData Create(
        IPropertySymbol symbol,
        RequiredAttributeOption requiredOption
    )
    {
        var containingTypeInfo = symbol.AnalyzeContainingTypes();

        return new DiagnosticData
        {
            Location = PropertyLocation.Create(symbol),
            ContainingTypeName = symbol.ContainingType.Name,
            IsContainingTypeSupported = containingTypeInfo.IsSupported,
            HasSetter = symbol.SetMethod != null,
            IsStatic = symbol.IsStatic,
            IsPartial = containingTypeInfo.IsPartial,
            HasSpecifiedMember = symbol.SpecifiedMemberExists(),
            RequiredOption = requiredOption,
        };
    }
}
