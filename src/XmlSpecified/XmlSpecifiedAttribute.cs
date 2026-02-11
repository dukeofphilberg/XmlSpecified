using System;

namespace XmlSpecified;

/// <summary>
/// Marks a property for automatic generation of a corresponding
/// [PropertyName]Specified property for XmlSerialization.
/// </summary>
/// <remarks>
/// <para>
/// When applied to a property in a partial class, the source generator will create
/// a getter-only boolean property named [PropertyName]Specified that returns a computed
/// value based on the main property's current state.
/// </para>
/// <para>
/// The generated property includes [XmlIgnore] to prevent it from being serialized as an element.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public partial class Order
/// {
///     [XmlSpecified(NumericOptions = NumericOptions.Positive)]
///     public int Quantity { get; set; }
/// }
///
/// // Generated:
/// // public bool QuantitySpecified => Quantity > 0;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class XmlSpecifiedAttribute : Attribute
{
    /// <summary>
    /// Specifies how numeric values determine the Specified state.
    /// Only applies to non-nullable numeric types. Must be explicitly set for int, long, decimal, double, etc.
    /// </summary>
    public NumericOptions? NumericOptions { get; init; }

    /// <summary>
    /// Specifies how string values determine the Specified state.
    /// Must be explicitly set for string properties.
    /// </summary>
    public StringOptions? StringOptions { get; init; }

    /// <summary>
    /// Specifies how boolean values determine the Specified state.
    /// Must be explicitly set for bool properties.
    /// </summary>
    public BoolOptions? BoolOptions { get; init; }

    /// <summary>
    /// Specifies how collection values determine the Specified state.
    /// Must be explicitly set for arrays, List&lt;T&gt;, ICollection&lt;T&gt;, etc.
    /// </summary>
    public CollectionOptions? CollectionOptions { get; init; }

    /// <summary>
    ///
    /// </summary>
    public XmlSpecifiedAttribute()
    {
        // Default constructor. All options are nullable and must be explicitly set by the user if needed.
    }

    /// <summary>
    /// Test
    /// </summary>
    /// <param name="numericOptions"></param>
    public XmlSpecifiedAttribute(NumericOptions numericOptions)
    {
        NumericOptions = numericOptions;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="stringOptions"></param>
    public XmlSpecifiedAttribute(StringOptions stringOptions)
    {
        StringOptions = stringOptions;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="boolOptions"></param>
    public XmlSpecifiedAttribute(BoolOptions boolOptions)
    {
        BoolOptions = boolOptions;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="collectionOptions"></param>
    public XmlSpecifiedAttribute(CollectionOptions collectionOptions)
    {
        CollectionOptions = collectionOptions;
    }
}
