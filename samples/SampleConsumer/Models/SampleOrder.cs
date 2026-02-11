using XmlSpecified;

namespace SampleConsumer.Models;

/// <summary>
/// Sample order model demonstrating XmlSpecifiedGenerator usage.
/// </summary>
public partial class SampleOrder
{
    /// <summary>
    /// Order ID - always serialized.
    /// </summary>
    public string OrderId { get; set; } = "";

    /// <summary>
    /// Customer name - always serialized.
    /// </summary>
    public string CustomerName { get; set; } = "";

    /// <summary>
    /// Optional quantity - serialized only when set (HasValue).
    /// </summary>
    [XmlSpecified]
    public int? Quantity { get; set; }

    /// <summary>
    /// Discount percentage - serialized only when positive.
    /// </summary>
    [XmlSpecified(NumericOptions.Positive)]
    public decimal Discount { get; set; }

    /// <summary>
    /// Unit price - serialized only when non-negative (zero or positive).
    /// </summary>
    [XmlSpecified(NumericOptions.Positive | NumericOptions.Zero)]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Shipping notes - serialized only when non-whitespace.
    /// </summary>
    [XmlSpecified(StringOptions.NonWhitespace)]
    public string ShippingNotes { get; set; } = "";

    /// <summary>
    /// Special instructions - serialized only when non-empty.
    /// </summary>
    [XmlSpecified(StringOptions.NonEmpty)]
    public string SpecialInstructions { get; set; } = "";

    /// <summary>
    /// Priority flag - serialized only when true.
    /// </summary>
    [XmlSpecified(BoolOptions.True)]
    public bool IsPriority { get; set; }

    /// <summary>
    /// Order date - serialized only when set (HasValue).
    /// </summary>
    [XmlSpecified]
    public DateTime? OrderDate { get; set; }

    /// <summary>
    /// Categories - serialized only when non-null.
    /// </summary>
    [XmlSpecified(CollectionOptions.NonNull)]
    public IEnumerable<string> Categories2 { get; set; } = null!;

    /// <summary>
    /// Categories - serialized only when non-null.
    /// </summary>
    [XmlSpecified(CollectionOptions.NonEmpty)]
    public IList<string> Categories32 { get; set; } = null!;
}
