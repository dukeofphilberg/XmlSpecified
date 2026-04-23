using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Models;

/// <summary>
/// Attribute values extracted from an XmlSpecified attribute.
/// </summary>
internal readonly record struct SpecifiedOptions(
    NumericOptions? NumericOptions,
    StringOptions? StringOptions,
    BoolOptions? BoolOptions,
    CollectionOptions? CollectionOptions
)
{
    internal static SpecifiedOptions Create(AttributeData attribute)
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

        return new SpecifiedOptions()
        {
            NumericOptions = numericOptions,
            StringOptions = stringOptions,
            BoolOptions = boolOptions,
            CollectionOptions = collectionOptions,
        };
    }
}
