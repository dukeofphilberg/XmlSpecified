using System.Linq;
using XmlSpecified.Generator.Models;

namespace XmlSpecified.Generator.Utilities;

internal static class Utilities
{
    /// <summary>
    /// Gets the generated file name for the PropertyContainer.
    /// </summary>
    internal static string GetFileName(PropertyContainer propertyContainer)
    {
        var str = propertyContainer.PropertyClasses.Select(p => p.Name);
        return $"{string.Join(".", str)}.XmlSpecified.g.cs";
    }
}
