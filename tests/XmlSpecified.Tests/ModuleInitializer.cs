using System.Runtime.CompilerServices;

namespace XmlSpecified.Tests;

public class TestInitializer
{
    [ModuleInitializer]
    public static void Init() => VerifySourceGenerators.Initialize();
}
