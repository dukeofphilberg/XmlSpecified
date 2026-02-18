using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Models;

/// <summary>
/// Attribute values extracted from an XmlSpecified attribute.
/// </summary>
internal readonly record struct AttributeValues(
    NumericOptions? NumericOptions,
    StringOptions? StringOptions,
    BoolOptions? BoolOptions,
    CollectionOptions? CollectionOptions
);
