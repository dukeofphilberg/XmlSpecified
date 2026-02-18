using Microsoft.CodeAnalysis;
using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

internal readonly record struct PropertyContainer(
    string Namespace,
    EquatableArray<PropertyClass> PropertyClasses
);
