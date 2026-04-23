using System.Runtime.CompilerServices;

namespace XmlSpecified.Tests;

public class TestInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();

        VerifierSettings.ScrubLinesWithReplace(line =>
        {
            if (line.Contains("global::System.CodeDom.Compiler.GeneratedCodeAttribute"))
            {
                return "    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"XmlSpecified\")]";
            }

            return line;
        });
    }
}
