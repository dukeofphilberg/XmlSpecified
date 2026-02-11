using XmlSpecified.Generator.Models;

namespace XmlSpecified.Generator.Utilities;

internal class ExpressionGenerator
{
    /// <summary>
    /// Generates the check expression based on property type and options.
    /// </summary>
    internal static string GenerateCheckExpression(PropertyData propertyData)
    {
        var propertyType = propertyData.PropertyType;
        var attributeValues = propertyData.AttributeValues;
        var propertyName = propertyData.PropertyName;

        // Check for nullable value type first
        if (propertyType.IsNullable)
        {
            // If a compatible option is specified, combine with HasValue
            if (attributeValues.NumericOptions is not null && propertyType.IsNumeric)
            {
                return GetNumericCheckExpression(
                    propertyName,
                    attributeValues.NumericOptions!.Value,
                    isNullable: true
                );
            }

            // Default nullable behavior
            return GetNullableDefaultCheckExpression(propertyName);
        }

        // Non-nullable types with explicit options
        if (attributeValues.NumericOptions is not null && propertyType.IsNumeric)
        {
            return GetNumericCheckExpression(
                propertyName,
                attributeValues.NumericOptions!.Value,
                isNullable: false
            );
        }

        if (attributeValues.StringOptions is not null && propertyType.IsString)
        {
            return GetStringCheckExpression(propertyName, attributeValues.StringOptions!.Value);
        }

        if (attributeValues.BoolOptions is not null && propertyType.IsBool)
        {
            return GetBoolCheckExpression(propertyName, attributeValues.BoolOptions!.Value);
        }

        if (attributeValues.CollectionOptions is not null && propertyType.IsArray)
        {
            return GetArrayCheckExpression(propertyName, attributeValues.CollectionOptions!.Value);
        }

        if (attributeValues.CollectionOptions is not null && propertyType.IsCollection)
        {
            return GetCollectionCheckExpression(
                propertyName,
                attributeValues.CollectionOptions!.Value
            );
        }

        if (attributeValues.CollectionOptions is not null && propertyType.IsEnumerable)
        {
            return GetEnumerableCheckExpression(
                propertyName,
                attributeValues.CollectionOptions!.Value
            );
        }

        // Reference type default (should only reach here for non-collection reference types)
        if (propertyType.IsReference)
        {
            return GetReferenceTypeDefaultCheckExpression(propertyName);
        }

        // Fallback (shouldn't reach here with proper validation)
        return $"{propertyName} != null";
    }

    /// <summary>
    /// Generates the check expression for numeric options.
    /// </summary>
    internal static string GetNumericCheckExpression(
        string propertyName,
        NumericOptions options,
        bool isNullable
    )
    {
        var valueAccess = isNullable ? $"{propertyName}.Value" : propertyName;
        var hasValue = isNullable ? $"{propertyName}.HasValue && " : "";

        // Handle all flag combinations
        var hasPositive = (options & NumericOptions.Positive) != 0;
        var hasZero = (options & NumericOptions.Zero) != 0;
        var hasNegative = (options & NumericOptions.Negative) != 0;

        // All three flags = always true
        if (hasPositive && hasZero && hasNegative)
        {
            return isNullable ? $"{propertyName}.HasValue" : "true";
        }

        // Two flags combinations
        if (hasPositive && hasZero)
        {
            return $"{hasValue}{valueAccess} >= 0";
        }

        if (hasNegative && hasZero)
        {
            return $"{hasValue}{valueAccess} <= 0";
        }

        if (hasPositive && hasNegative)
        {
            return $"{hasValue}{valueAccess} != 0";
        }

        // Single flag
        if (hasPositive)
        {
            return $"{hasValue}{valueAccess} > 0";
        }

        if (hasZero)
        {
            return $"{hasValue}{valueAccess} == 0";
        }

        if (hasNegative)
        {
            return $"{hasValue}{valueAccess} < 0";
        }

        // No flags set - treat as error case, return false
        return "false";
    }

    /// <summary>
    /// Generates the check expression for string options.
    /// </summary>
    internal static string GetStringCheckExpression(string propertyName, StringOptions options)
    {
        return options switch
        {
            StringOptions.NonWhitespace => $"!string.IsNullOrWhiteSpace({propertyName})",
            StringOptions.NonEmpty => $"!string.IsNullOrEmpty({propertyName})",
            StringOptions.NonNull => $"{propertyName} != null",
            _ => $"!string.IsNullOrWhiteSpace({propertyName})",
        };
    }

    /// <summary>
    /// Generates the check expression for bool options.
    /// </summary>
    internal static string GetBoolCheckExpression(string propertyName, BoolOptions options)
    {
        return options switch
        {
            BoolOptions.True => propertyName,
            BoolOptions.False => $"!{propertyName}",
            _ => propertyName,
        };
    }

    /// <summary>
    /// Generates the check expression for collection options.
    /// </summary>
    internal static string GetArrayCheckExpression(string propertyName, CollectionOptions options)
    {
        return options switch
        {
            CollectionOptions.NonNull => $"{propertyName} != null",
            _ => $"{propertyName} != null && {propertyName}.Length > 0",
        };
    }

    /// <summary>
    /// Generates the check expression for collection options.
    /// </summary>
    internal static string GetCollectionCheckExpression(
        string propertyName,
        CollectionOptions options
    )
    {
        return options switch
        {
            CollectionOptions.NonNull => $"{propertyName} != null",
            _ => $"{propertyName} != null && {propertyName}.Count > 0",
        };
    }

    /// <summary>
    /// Generates the default check expression for nullable value types.
    /// </summary>
    internal static string GetNullableDefaultCheckExpression(string propertyName) =>
        $"{propertyName}.HasValue";

    /// <summary>
    /// Generates the default check expression for reference types.
    /// </summary>
    internal static string GetReferenceTypeDefaultCheckExpression(string propertyName) =>
        $"{propertyName} != null";

    /// <summary>
    /// Generates the check expression for Enumerable.
    /// </summary>
    internal static string GetEnumerableCheckExpression(
        string propertyName,
        CollectionOptions options
    )
    {
        return options switch
        {
            CollectionOptions.NonNull => $"{propertyName} != null",
            _ => $"{propertyName} != null && {propertyName}.Any()",
        };
    }
}
