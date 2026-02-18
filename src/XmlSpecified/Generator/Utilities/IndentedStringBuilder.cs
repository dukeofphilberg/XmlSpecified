using System.Text;

namespace XmlSpecified.Generator.Utilities;

internal sealed class IndentedStringBuilder
{
    private readonly StringBuilder _builder;
    private int _indentLevel;
    private string _indentString;

    internal IndentedStringBuilder()
    {
        _builder = new StringBuilder();
        _indentLevel = 0;
        _indentString = string.Empty;
    }

    internal void IncreaseIndent()
    {
        _indentLevel++;
        _indentString = new string(' ', _indentLevel * 4);
    }

    internal void DecreaseIndent()
    {
        if (_indentLevel > 0)
        {
            _indentLevel--;
            _indentString = new string(' ', _indentLevel * 4);
        }
    }

    internal void AppendLine(string line) => _builder.AppendLine(_indentString + line);

    internal void AppendLine() => _builder.AppendLine();

    public override string ToString() => _builder.ToString();
}
