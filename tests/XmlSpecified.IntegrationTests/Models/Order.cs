namespace XmlSpecified.IntegrationTests.Models;

/// <summary>
/// Test model for XmlSerializer integration tests.
/// </summary>
public partial class Order
{
    /// <summary>
    /// Order identifier - always serialized.
    /// </summary>
    public string OrderId { get; set; } = "";

    /// <summary>
    /// Nullable quantity - serialized only when HasValue is true.
    /// </summary>
    [XmlSpecified]
    public int? Quantity { get; set; }

    /// <summary>
    /// Discount amount - serialized only when positive.
    /// </summary>
    [XmlSpecified(NumericOptions.Positive)]
    public decimal Discount { get; set; }

    /// <summary>
    /// Notes - serialized only when non-whitespace.
    /// </summary>
    [XmlSpecified(StringOptions.NonWhitespace)]
    public string Notes { get; set; } = "";

    /// <summary>
    /// Urgency flag - serialized only when true.
    /// </summary>
    [XmlSpecified(BoolOptions.True)]
    public bool IsUrgent { get; set; }
}
