using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct PropertyClass
{
    public string Keyword { get; init; }
    public string Name { get; init; }
    public string Constraints { get; init; }

    public static EquatableArray<PropertyClass> Construct(IPropertySymbol propertySymbol)
    {
        var parentClasses = new List<PropertyClass>();

        // Start with the immediate containing type
        var containingType = propertySymbol.ContainingType;

        // Keep looping while we're in a supported nested type
        while (containingType != null && IsAllowedKind(containingType.TypeKind))
        {
            // Build the keyword (class/struct/record)
            var keyword = GetTypeKeyword(containingType);

            // Build the name with type parameters if generic
            var name = containingType.Name;
            if (containingType.TypeParameters.Length > 0)
            {
                name += "<" + string.Join(", ", containingType.TypeParameters) + ">";
            }

            // Extract constraints from type parameters
            var constraints = GetTypeConstraints(containingType);

            parentClasses.Add(new PropertyClass
            {
                Keyword = keyword,
                Name = name,
                Constraints = constraints,
            });

            // Move to the next outer type
            containingType = containingType.ContainingType;
        }

        // Reverse so outermost class is first
        parentClasses.Reverse();

        return new EquatableArray<PropertyClass>(parentClasses.ToArray());
    }

    private static bool IsAllowedKind(TypeKind kind) =>
        kind == TypeKind.Class || kind == TypeKind.Struct;

    private static string GetTypeKeyword(INamedTypeSymbol type)
    {
        if (type.IsRecord)
        {
            return type.TypeKind == TypeKind.Struct ? "record struct" : "record class";
        }

        return type.TypeKind == TypeKind.Struct ? "struct" : "class";
    }

    private static string GetTypeConstraints(INamedTypeSymbol type)
    {
        if (type.TypeParameters.Length == 0)
        {
            return string.Empty;
        }

        var constraints = new List<string>();

        foreach (var typeParam in type.TypeParameters)
        {
            var paramConstraints = new List<string>();

            // Reference type constraint
            if (typeParam.HasReferenceTypeConstraint)
            {
                paramConstraints.Add("class");
            }

            // Value type constraint
            if (typeParam.HasValueTypeConstraint)
            {
                paramConstraints.Add("struct");
            }

            // Unmanaged constraint
            if (typeParam.HasUnmanagedTypeConstraint)
            {
                paramConstraints.Add("unmanaged");
            }

            // Not null constraint
            if (typeParam.HasNotNullConstraint)
            {
                paramConstraints.Add("notnull");
            }

            // Type constraints (base class, interfaces)
            foreach (var constraintType in typeParam.ConstraintTypes)
            {
                paramConstraints.Add(constraintType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
            }

            // Constructor constraint (must come last)
            if (typeParam.HasConstructorConstraint)
            {
                paramConstraints.Add("new()");
            }

            if (paramConstraints.Count > 0)
            {
                constraints.Add($"where {typeParam.Name} : {string.Join(", ", paramConstraints)}");
            }
        }

        return string.Join(" ", constraints);
    }
}
