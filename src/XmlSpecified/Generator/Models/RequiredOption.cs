using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct RequiredOption(bool IsRequired, string OptionName)
{
    /// <summary>
    /// Determines the required option type for a property type.
    /// Returns null if no explicit option is required (nullable value types, reference types).
    /// </summary>
    internal static RequiredOption Create(IPropertySymbol symbol, SpecifiedOptions attributeValues)
    {
        var propertyType = symbol.Type;
        string requiredOptionName = string.Empty;

        // Nullable value types don't require explicit options
        if (TypeCheckResolver.IsNullableValueType(propertyType))
        {
            requiredOptionName = string.Empty;
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
            requiredOptionName = string.Empty;
        }

        var hasRequiredOption = requiredOptionName switch
        {
            "NumericOptions" => attributeValues.NumericOptions is not null,
            "StringOptions" => attributeValues.StringOptions is not null,
            "BoolOptions" => attributeValues.BoolOptions is not null,
            "CollectionOptions" => attributeValues.CollectionOptions is not null,
            _ => true,
        };

        return new RequiredOption(hasRequiredOption, requiredOptionName);
    }
}
