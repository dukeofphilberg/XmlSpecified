using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Diagnostics;

/// <summary>
/// Defines all diagnostic descriptors for the XmlSpecifiedGenerator.
/// </summary>
internal static class DiagnosticDescriptors
{
    private const string CategoryGenerator = "XmlSpecifiedGenerator";
    private const string CategoryUsage = "Usage";

    internal static readonly DiagnosticDescriptor GeneratorError = new(
        id: "XSG000",
        title: "Generator error",
        messageFormat: "XmlSpecifiedGenerator failed: {0}",
        category: CategoryGenerator,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "An unexpected error occurred during source generation."
    );

    /// <summary>
    /// XSG001: Property is decorated with [XmlSpecified] but class is not partial.
    /// </summary>
    internal static readonly DiagnosticDescriptor NonPartialClass = new(
        id: "XSG001",
        title: "Non-partial class",
        messageFormat: "Property '{0}' is decorated with [XmlSpecified] but class '{1}' is not partial. Add the 'partial' modifier to the class declaration.",
        category: CategoryUsage,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Classes containing properties decorated with [XmlSpecified] must be declared as partial to allow the source generator to add the Specified property."
    );

    /// <summary>
    /// XSG002: A member with the Specified name already exists.
    /// </summary>
    internal static readonly DiagnosticDescriptor DuplicateSpecifiedProperty = new(
        id: "XSG002",
        title: "Duplicate Specified member",
        messageFormat: "A member named '{0}Specified' already exists in class '{1}'. The generator will not create a duplicate.",
        category: CategoryUsage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "A member (property, field, method, ...) with the expected Specified name already exists in the class. The generator will skip generation for this property."
    );

    /// <summary>
    /// XSG003: Property has no setter.
    /// </summary>
    internal static readonly DiagnosticDescriptor ReadOnlyProperty = new(
        id: "XSG003",
        title: "Read-only property",
        messageFormat: "Property '{0}' has no setter. XmlSerializer requires setters for deserialization.",
        category: CategoryUsage,
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
        category: CategoryUsage,
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
        category: CategoryUsage,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Non-nullable value types (bool, int, string, collections) require an explicit option to determine when they are considered 'specified'."
    );

    /// <summary>
    /// XSG006: Property type is not supported.
    /// </summary>
    internal static readonly DiagnosticDescriptor UnsupportedPropertyType = new(
        id: "XSG006",
        title: "Unsupported property type",
        messageFormat: "Property '{0}' of type '{1}' is not supported. Use a nullable type or write the Specified property manually.",
        category: CategoryUsage,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Non-nullable value types other than bool and numeric types (e.g. enums, DateTime, Guid, structs) have no 'unspecified' state the generator can check."
    );

    /// <summary>
    /// XSG007: Containing type is not supported.
    /// </summary>
    internal static readonly DiagnosticDescriptor UnsupportedContainingType = new(
        id: "XSG007",
        title: "Unsupported containing type",
        messageFormat: "Property '{0}' is declared in '{1}', which is not a class or struct, or is nested in a type that is not. [XmlSpecified] is only supported on class and struct members.",
        category: CategoryUsage,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The generator can only add Specified properties to classes, structs and records, including nested ones."
    );
}
