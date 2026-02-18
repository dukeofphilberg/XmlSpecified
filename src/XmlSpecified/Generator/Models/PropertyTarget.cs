using Microsoft.CodeAnalysis;

namespace XmlSpecified.Generator.Models;

internal readonly record struct PropertyTarget(
    PropertyContainer PropertyContainer,
    PropertyData PropertyData,
    DiagnosticData DiagnosticData
);
