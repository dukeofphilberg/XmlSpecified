namespace XmlSpecified.Generator.Models;

/// <summary>
/// Result of checking the containing type of a property, and every type it is nested in.
/// </summary>
/// <param name="IsPartial">Whether all checked types are partial.</param>
/// <param name="IsSupported">Whether all checked types are a class or struct.</param>
internal readonly record struct ContainingTypeInfo(bool IsPartial, bool IsSupported);
