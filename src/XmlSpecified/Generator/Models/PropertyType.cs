using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct PropertyType(
    bool IsNullable,
    bool IsNumeric,
    bool IsReference,
    bool IsString,
    bool IsBool,
    bool IsArray,
    bool IsCollection,
    bool IsEnumerable
);
