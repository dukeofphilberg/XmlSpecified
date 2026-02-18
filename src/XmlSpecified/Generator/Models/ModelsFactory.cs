using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal static class ModelsFactory
{
    public static AttributeValues Create(AttributeData attribute)
    {
        NumericOptions? numericOptions = null;
        StringOptions? stringOptions = null;
        BoolOptions? boolOptions = null;
        CollectionOptions? collectionOptions = null;

        const string numericOptionsName = nameof(NumericOptions);
        const string stringOptionsName = nameof(StringOptions);
        const string boolOptionsName = nameof(BoolOptions);
        const string collectionOptionsName = nameof(CollectionOptions);

        foreach (var namedArg in attribute.ConstructorArguments)
        {
            switch (namedArg.Type?.Name)
            {
                case numericOptionsName:
                    numericOptions = (NumericOptions)(int)namedArg.Value!;
                    break;
                case stringOptionsName:
                    stringOptions = (StringOptions)(int)namedArg.Value!;
                    break;
                case boolOptionsName:
                    boolOptions = (BoolOptions)(int)namedArg.Value!;
                    break;
                case collectionOptionsName:
                    collectionOptions = (CollectionOptions)(int)namedArg.Value!;
                    break;
            }
        }

        return new AttributeValues()
        {
            NumericOptions = numericOptions,
            StringOptions = stringOptions,
            BoolOptions = boolOptions,
            CollectionOptions = collectionOptions,
        };
    }

    internal static PropertyLocation CreatePropertyLocation(IPropertySymbol symbol)
    {
        var location = symbol.Locations[0];
        var lineSpan = location.GetLineSpan();

        return new PropertyLocation
        {
            FilePath = lineSpan.Path,
            TextSpan = location.SourceSpan,
            LineSpan = lineSpan.Span,
        };
    }

    internal static PropertyType Create(ITypeSymbol propertyType)
    {
        return new PropertyType
        {
            IsNullable = TypeCheckResolver.IsNullableValueType(propertyType),
            IsNumeric = TypeCheckResolver.IsNumericType(propertyType),
            IsReference = TypeCheckResolver.IsReferenceType(propertyType),
            IsString = TypeCheckResolver.IsStringType(propertyType),
            IsBool = TypeCheckResolver.IsBoolType(propertyType),
            IsArray = propertyType is IArrayTypeSymbol,
            IsCollection = TypeCheckResolver.IsCollectionType(propertyType),
            IsEnumerable = TypeCheckResolver.IsEnumerable(propertyType),
        };
    }

    internal static PropertyTarget CreatePropertyTarget(GeneratorAttributeSyntaxContext context)
    {
        var symbol = (IPropertySymbol)context.TargetSymbol;
        var attributes = symbol.GetAttributes();
        var attributeValues = Create(attributes[0]);

        return new PropertyTarget
        {
            PropertyContainer = CreatePropertyContainer(symbol),
            PropertyData = CreatePropertyData(symbol, attributeValues),
            DiagnosticData = CreateDiagnosticData(symbol, attributeValues),
        };
    }

    internal static PropertyData CreatePropertyData(
        IPropertySymbol symbol,
        AttributeValues attributeValues
    )
    {
        return new PropertyData
        {
            AttributeValues = attributeValues,
            PropertyName = symbol.Name,
            PropertyType = Create(symbol.Type),
            PropertyTypeDisplayString = symbol.Type.ToDisplayString(),
        };
    }

    internal static PropertyContainer CreatePropertyContainer(IPropertySymbol symbol)
    {
        return new PropertyContainer
        {
            Namespace =
                symbol.ContainingNamespace.ToDisplayString(
                    SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(
                        SymbolDisplayGlobalNamespaceStyle.Omitted
                    )
                ) ?? string.Empty,
            PropertyClasses = Construct(symbol),
        };
    }

    internal static DiagnosticData CreateDiagnosticData(
        IPropertySymbol symbol,
        AttributeValues attributeValues
    )
    {
        var (requiredOptionName, hasRequiredOption) = GetHasRequiredOption(symbol, attributeValues);

        return new DiagnosticData
        {
            Location = CreatePropertyLocation(symbol),
            HasSetter = symbol.SetMethod != null,
            IsStatic = symbol.IsStatic,
            IsPartial = IsContainingClassPartial(symbol),
            HasSpecifiedPropertyExists = SpecifiedPropertyExists(symbol),
            RequiredOptionName = requiredOptionName,
            HasRequiredOption = hasRequiredOption,
        };
    }

    /// <summary>
    /// Checks if the containing class is partial.
    /// </summary>
    internal static bool IsContainingClassPartial(IPropertySymbol symbol)
    {
        var containingType = symbol.ContainingType;

        // Check all declarations of the type
        foreach (var syntaxRef in containingType.DeclaringSyntaxReferences)
        {
            if (syntaxRef.GetSyntax() is TypeDeclarationSyntax typeDecl)
            {
                foreach (var modifier in typeDecl.Modifiers)
                {
                    if (modifier.Text == "partial")
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if a Specified property already exists for the given property name.
    /// </summary>
    internal static bool SpecifiedPropertyExists(IPropertySymbol symbol)
    {
        var specifiedName = $"{symbol.Name}Specified";
        var containingType = symbol.ContainingType;

        foreach (var member in containingType.GetMembers())
        {
            if (member is IPropertySymbol prop && prop.Name == specifiedName)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines the required option type for a property type.
    /// Returns null if no explicit option is required (nullable value types, reference types).
    /// </summary>
    internal static (string?, bool) GetHasRequiredOption(
        IPropertySymbol symbol,
        AttributeValues attributeValues
    )
    {
        var propertyType = symbol.Type;
        string? requiredOptionName = string.Empty;

        // Nullable value types don't require explicit options
        if (TypeCheckResolver.IsNullableValueType(propertyType))
        {
            requiredOptionName = null;
        }
        // Non-nullable value types require explicit options
        else if (TypeCheckResolver.IsBoolType(propertyType))
        {
            requiredOptionName = "BoolOptions";
        }
        else if (TypeCheckResolver.IsNumericType(propertyType))
        {
            requiredOptionName = "NumericOptions";
        }
        // String requires StringOptions
        else if (TypeCheckResolver.IsStringType(propertyType))
        {
            requiredOptionName = "StringOptions";
        }
        // Collections require CollectionOptions
        else if (
            propertyType.TypeKind == TypeKind.Array
            || TypeCheckResolver.IsCollectionType(propertyType)
            || TypeCheckResolver.IsEnumerable(propertyType)
        )
        {
            requiredOptionName = "CollectionOptions";
        }
        else
        {
            // For other reference types, no explicit options are required
            requiredOptionName = null;
        }

        var hasRequiredOption = requiredOptionName switch
        {
            "NumericOptions" => attributeValues.NumericOptions is not null,
            "StringOptions" => attributeValues.StringOptions is not null,
            "BoolOptions" => attributeValues.BoolOptions is not null,
            "CollectionOptions" => attributeValues.CollectionOptions is not null,
            _ => true,
        };

        return (requiredOptionName, hasRequiredOption);
    }

    internal static EquatableArray<PropertyClass> Construct(IPropertySymbol propertySymbol)
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

            parentClasses.Add(new PropertyClass { Keyword = keyword, Name = name });

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
                paramConstraints.Add(
                    constraintType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
                );
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
