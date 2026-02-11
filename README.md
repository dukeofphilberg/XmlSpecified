# XmlSpecified

A C# source generator that automatically creates `[PropertyName]Specified` properties for XmlSerialization, eliminating boilerplate code for conditional XML element inclusion.

## Installation

```bash
dotnet add package XmlSpecified
```

## Quick Start

```csharp
using XmlSpecified;

public partial class Order  // Must be partial!
{
    [XmlSpecified(NumericOptions = NumericOptions.Positive)]
    public int Quantity { get; set; }

    [XmlSpecified(StringOptions = StringOptions.NonWhitespace)]
    public string CustomerName { get; set; }

    [XmlSpecified]  // Nullable types default to HasValue
    public DateTime? ShipDate { get; set; }
}
```

The source generator automatically creates:

```csharp
public partial class Order
{
    [XmlIgnore]
    public bool QuantitySpecified => Quantity > 0;

    [XmlIgnore]
    public bool CustomerNameSpecified => !string.IsNullOrWhiteSpace(CustomerName);

    [XmlIgnore]
    public bool ShipDateSpecified => ShipDate.HasValue;
}
```

## Features

- **Automatic generation** of `Specified` properties for XmlSerializer
- **Type-specific options**: `NumericOptions`, `StringOptions`, `BoolOptions`, `CollectionOptions`
- **Nullable support**: Automatic `HasValue` checks for nullable types
- **Compile-time diagnostics** for invalid usage
- **Zero runtime overhead** - all generation happens at compile time

## Configuration Options

### NumericOptions (Flags)

```csharp
[XmlSpecified(NumericOptions = NumericOptions.Positive)]  // value > 0
[XmlSpecified(NumericOptions = NumericOptions.Positive | NumericOptions.Zero)]  // value >= 0
[XmlSpecified(NumericOptions = NumericOptions.Negative)]  // value < 0
```

### StringOptions

```csharp
[XmlSpecified(StringOptions = StringOptions.NonWhitespace)]  // !IsNullOrWhiteSpace
[XmlSpecified(StringOptions = StringOptions.NonEmpty)]  // !IsNullOrEmpty
[XmlSpecified(StringOptions = StringOptions.NonNull)]  // != null
```

### BoolOptions

```csharp
[XmlSpecified(BoolOptions = BoolOptions.True)]   // value == true
[XmlSpecified(BoolOptions = BoolOptions.False)]  // value == false
```

### CollectionOptions

```csharp
[XmlSpecified(CollectionOptions = CollectionOptions.NonEmpty)]  // != null && Count > 0
[XmlSpecified(CollectionOptions = CollectionOptions.NonNull)]   // != null
```

## Diagnostics

| Code | Severity | Description |
|------|----------|-------------|
| XSG001 | Error | Class must be partial |
| XSG002 | Warning | Specified property already exists |
| XSG003 | Warning | Property has no setter |
| XSG004 | Error | Property is static |
| XSG005 | Warning | Multiple option types specified |
| XSG006 | Error | Missing required option |

## License

MIT
