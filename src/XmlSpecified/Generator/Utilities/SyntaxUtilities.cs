using Microsoft.CodeAnalysis.CSharp;

namespace XmlSpecified.Generator.Utilities;

internal static class SyntaxUtilities
{
    /// <summary>
    /// Prefixes the identifier with '@' when it is a C# keyword (e.g. <c>class</c> becomes <c>@class</c>).
    /// </summary>
    internal static string EscapeIdentifier(string identifier) =>
        SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None ? "@" + identifier : identifier;
}
