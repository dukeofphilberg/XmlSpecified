using System.Linq;
using XmlSpecified.Generator.Models;

namespace XmlSpecified.Generator.Utilities;

internal static class GeneratorUtilities
{
    /// <summary>
    /// Gets the generated file name for given <see cref="PropertyContainer"/>.
    /// Includes the namespace and generic arity so the name is unique and a valid hint name.
    /// </summary>
    internal static string GetFileName(PropertyContainer propertyContainer)
    {
        var parts = propertyContainer.PropertyClasses.Select(p => p.MetadataName);

        if (!string.IsNullOrEmpty(propertyContainer.Namespace))
        {
            parts = parts.Prepend(propertyContainer.Namespace);
        }

        return $"{string.Join(".", parts)}.XmlSpecified.g.cs";
    }
}
