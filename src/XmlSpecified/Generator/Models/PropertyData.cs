namespace XmlSpecified.Generator.Models;

/// <summary>
/// Data about a property decorated with XmlSpecified.
/// </summary>
internal readonly record struct PropertyData(
    string PropertyName,
    PropertyType PropertyType,
    string PropertyTypeDisplayString,
    AttributeValues AttributeValues
);
