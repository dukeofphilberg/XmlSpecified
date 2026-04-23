using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace XmlSpecified.Generator.Models;

internal readonly record struct PropertyLocation(
    string FilePath,
    TextSpan TextSpan,
    LinePositionSpan LineSpan
) : IEquatable<PropertyLocation>
{
    internal static PropertyLocation Create(IPropertySymbol symbol)
    {
        var location = symbol.Locations[0];
        var lineSpan = location.GetLineSpan();

        return new PropertyLocation
        {
            FilePath = lineSpan.Path,
            TextSpan = location.SourceSpan,
            LineSpan = lineSpan.Span,
        };
    }

    internal Location GetLocation() => Location.Create(FilePath, TextSpan, LineSpan);

    public bool Equals(PropertyLocation other)
    {
        return FilePath == other.FilePath
            && TextSpan.Equals(other.TextSpan)
            && LineSpan.Equals(other.LineSpan);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 31 + (FilePath?.GetHashCode() ?? 0);
            hash = hash * 31 + TextSpan.GetHashCode();
            hash = hash * 31 + LineSpan.GetHashCode();
            return hash;
        }
    }
}
