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
    internal static RequiredOption Create(TypeInfo typeInfo, SpecifiedOptions attributeValues)
    {
        // Non-nullable value types require explicit options
        if (typeInfo.IsBool)
        {
            return new RequiredOption(attributeValues.BoolOptions is not null, "BoolOptions");
        }
        else if (typeInfo.IsNumeric)
        {
            return new RequiredOption(attributeValues.NumericOptions is not null, "NumericOptions");
        }
        // String requires StringOptions
        else if (typeInfo.IsString)
        {
            return new RequiredOption(attributeValues.StringOptions is not null, "StringOptions");
        }
        // Collections require CollectionOptions
        else if (typeInfo.IsArray || typeInfo.IsCollection || typeInfo.IsEnumerable)
        {
            return new RequiredOption(
                attributeValues.CollectionOptions is not null,
                "CollectionOptions"
            );
        }

        // For other reference types or nullable types, no explicit options are required
        return new RequiredOption(true, string.Empty);
    }
}
