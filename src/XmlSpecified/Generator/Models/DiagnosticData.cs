using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct DiagnosticData(
    PropertyLocation Location,
    bool HasSetter,
    bool IsStatic,
    bool IsPartial,
    bool HasSpecifiedPropertyExists,
    string? RequiredOptionName,
    bool HasRequiredOption
);
