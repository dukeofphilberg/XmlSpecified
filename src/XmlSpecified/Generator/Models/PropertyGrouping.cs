using XmlSpecified.Generator.Utilities;

namespace XmlSpecified.Generator.Models;

/// <summary>
/// Grouped pipeline output — a named equatable type to replace anonymous ValueTuple
/// in the incremental generator pipeline, which requires proper equality support.
/// </summary>
internal readonly record struct PropertyGrouping
{
    public PropertyContainer Container { get; init; }
    public EquatableArray<(PropertyData, DiagnosticData)> Properties { get; init; }
}
