using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Diagnostics;

/// <summary>
/// Defines all diagnostic descriptors for the XmlSpecifiedGenerator.
/// </summary>
internal static class DiagnosticDescriptors
{
    private const string Category = "Usage";

    /// <summary>
    /// XSG001: Property is decorated with [XmlSpecified] but class is not partial.
    /// </summary>
    internal static readonly DiagnosticDescriptor NonPartialClass = new(
        id: "XSG001",
        title: "Non-partial class",
        messageFormat: "Property '{0}' is decorated with [XmlSpecified] but class '{1}' is not partial. Add the 'partial' modifier to the class declaration.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Classes containing properties decorated with [XmlSpecified] must be declared as partial to allow the source generator to add the Specified property."
    );

    /// <summary>
    /// XSG002: A Specified property already exists.
    /// </summary>
    internal static readonly DiagnosticDescriptor DuplicateSpecifiedProperty = new(
        id: "XSG002",
        title: "Duplicate Specified property",
        messageFormat: "Property '{0}Specified' already exists in class '{1}'. The generator will not create a duplicate.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "A property with the expected Specified name already exists in the class. The generator will skip generation for this property."
    );

    /// <summary>
    /// XSG003: Property has no setter.
    /// </summary>
    internal static readonly DiagnosticDescriptor ReadOnlyProperty = new(
        id: "XSG003",
        title: "Read-only property",
        messageFormat: "Property '{0}' has no setter. XmlSerializer requires setters for deserialization.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "XmlSerializer requires properties to have setters for deserialization. Consider adding a setter or using a different serialization approach."
    );

    /// <summary>
    /// XSG004: Property is static.
    /// </summary>
    internal static readonly DiagnosticDescriptor StaticProperty = new(
        id: "XSG004",
        title: "Static property",
        messageFormat: "Property '{0}' is static. XmlSerializer ignores static members. Remove [XmlSpecified] or make the property non-static.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "XmlSerializer does not serialize static members. The [XmlSpecified] attribute should only be applied to instance properties."
    );

    /// <summary>
    /// XSG005: Missing required option for non-nullable type.
    /// </summary>
    internal static readonly DiagnosticDescriptor MissingRequiredAttributeOption = new(
        id: "XSG005",
        title: "Missing required attribute option",
        messageFormat: "Property '{0}' of type '{1}' requires {2} to be specified",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Non-nullable value types (bool, int, string, collections) require an explicit option to determine when they are considered 'specified'."
    );
}
