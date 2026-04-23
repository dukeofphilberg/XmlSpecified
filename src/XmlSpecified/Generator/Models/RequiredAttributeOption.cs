namespace XmlSpecified.Generator.Models;

/// <summary>
/// Determines the <see cref="AttributeOptions"/> required for a <see cref="TypeInfo"/>.
/// Returns true if the option is present, false otherwise.
/// </summary>
internal readonly record struct RequiredAttributeOption(bool IsPresent, string RequiredOptionsName)
{
    internal static RequiredAttributeOption Create(
        TypeInfo typeInfo,
        AttributeOptions attributeValues
    )
    {
        if (typeInfo.IsBool)
        {
            return new RequiredAttributeOption(
                attributeValues.BoolOptions is not null,
                nameof(attributeValues.BoolOptions)
            );
        }
        else if (typeInfo.IsNumeric)
        {
            return new RequiredAttributeOption(
                attributeValues.NumericOptions is not null,
                nameof(attributeValues.NumericOptions)
            );
        }
        else if (typeInfo.IsString)
        {
            return new RequiredAttributeOption(
                attributeValues.StringOptions is not null,
                nameof(attributeValues.StringOptions)
            );
        }
        else if (typeInfo.IsArray || typeInfo.IsCollection || typeInfo.IsEnumerable)
        {
            return new RequiredAttributeOption(
                attributeValues.CollectionOptions is not null,
                nameof(attributeValues.CollectionOptions)
            );
        }

        // For other reference types or nullable types, no explicit options are required
        return new RequiredAttributeOption(true, string.Empty);
    }
}
