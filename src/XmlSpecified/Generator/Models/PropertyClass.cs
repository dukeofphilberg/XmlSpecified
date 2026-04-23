using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct PropertyClass(string Keyword, string Name)
{
    internal static EquatableArray<PropertyClass> Construct(IPropertySymbol propertySymbol)
    {
        var parentClasses = new List<PropertyClass>();

        // Start with the immediate containing type
        var containingType = propertySymbol.ContainingType;

        // Keep looping while we're in a supported nested type
        while (containingType != null && IsAllowedKind(containingType.TypeKind))
        {
            // Build the keyword (class/struct/record)
            var keyword = TypeUtilities.GetTypeKeyword(containingType);

            // Build the name with type parameters if generic
            var name = containingType.Name;
            if (containingType.TypeParameters.Length > 0)
            {
                name += "<" + string.Join(", ", containingType.TypeParameters) + ">";
            }

            // Extract constraints from type parameters
            var constraints = TypeUtilities.GetTypeConstraints(containingType);

            parentClasses.Add(new PropertyClass { Keyword = keyword, Name = name });

            // Move to the next outer type
            containingType = containingType.ContainingType;
        }

        // Reverse so outermost class is first
        parentClasses.Reverse();

        return new EquatableArray<PropertyClass>(parentClasses.ToArray());
    }

    private static bool IsAllowedKind(TypeKind kind) => kind is TypeKind.Class or TypeKind.Struct;
}
