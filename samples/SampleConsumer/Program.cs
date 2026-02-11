using System.Xml.Serialization;
using SampleConsumer.Models;

Console.WriteLine("XmlSpecifiedGenerator Sample Consumer");
Console.WriteLine("=====================================\n");

// Create sample order with various optional values
var order = new SampleOrder
{
    OrderId = "ORD-2024-001",
    CustomerName = "John Doe",
    Quantity = 5,
    Discount = 10.5m, // Positive, will be serialized
    UnitPrice = 0m, // Zero, will be serialized (NonNegative option)
    ShippingNotes = "Handle with care",
    SpecialInstructions = "", // Empty, will NOT be serialized
    IsPriority = true, // True, will be serialized
    OrderDate = DateTime.Now,
};

Console.WriteLine("Order with values set:");
Console.WriteLine(SerializeToXml(order));
Console.WriteLine();

// Create order with default/unset values
var minimalOrder = new SampleOrder
{
    OrderId = "ORD-2024-002",
    CustomerName = "Jane Smith",
    Quantity = null, // Null, will NOT be serialized
    Discount = 0m, // Zero, will NOT be serialized (Positive option)
    UnitPrice = -1m, // Negative, will NOT be serialized (NonNegative option)
    ShippingNotes = "   ", // Whitespace only, will NOT be serialized
    SpecialInstructions = "", // Empty, will NOT be serialized
    IsPriority = false, // False, will NOT be serialized
    OrderDate = null, // Null, will NOT be serialized
};

Console.WriteLine("Minimal order (only required fields serialized):");
Console.WriteLine(SerializeToXml(minimalOrder));

static string SerializeToXml(SampleOrder order)
{
    var serializer = new XmlSerializer(typeof(SampleOrder));
    using var writer = new StringWriter();
    var ns = new XmlSerializerNamespaces();
    ns.Add("", ""); // Remove namespace prefixes
    serializer.Serialize(writer, order, ns);
    return writer.ToString();
}
