using System.Diagnostics;

namespace XmlSpecified.Tests.GeneratorTests;

public class PerformanceTests
{
    [Fact]
    public void Generator_With100Properties_CompletesInUnder2Seconds()
    {
        // Arrange
        var source = GenerateSourceWith100Properties();
        var stopwatch = Stopwatch.StartNew();

        // Act
        _ = GeneratorTestHelper.GetDriver(source);
        stopwatch.Stop();

        // Assert
        Assert.True(
            stopwatch.Elapsed.TotalSeconds < 2.0,
            $"Generation took {stopwatch.Elapsed.TotalSeconds:F2} seconds, expected < 2 seconds"
        );
    }

    private static string GenerateSourceWith100Properties()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("using XmlSpecified;");
        sb.AppendLine();
        sb.AppendLine("namespace PerformanceTest;");
        sb.AppendLine();
        sb.AppendLine("public partial class LargeModel");
        sb.AppendLine("{");

        for (int i = 1; i <= 100; i++)
        {
            // Alternate between different types to test various code paths
            switch (i % 5)
            {
                case 0:
                    sb.AppendLine($"    [XmlSpecified]");
                    sb.AppendLine($"    public int? Property{i:D3} {{ get; set; }}");
                    break;
                case 1:
                    sb.AppendLine($"    [XmlSpecified(NumericOptions.Positive)]");
                    sb.AppendLine($"    public int Property{i:D3} {{ get; set; }}");
                    break;
                case 2:
                    sb.AppendLine($"    [XmlSpecified(StringOptions.NonWhitespace)]");
                    sb.AppendLine($"    public string Property{i:D3} {{ get; set; }}");
                    break;
                case 3:
                    sb.AppendLine($"    [XmlSpecified(BoolOptions.True)]");
                    sb.AppendLine($"    public bool Property{i:D3} {{ get; set; }}");
                    break;
                case 4:
                    sb.AppendLine($"    [XmlSpecified]");
                    sb.AppendLine($"    public System.DateTime? Property{i:D3} {{ get; set; }}");
                    break;
            }
            sb.AppendLine();
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
}
