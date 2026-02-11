using System;

namespace XmlSpecified;

/// <summary>
/// Flags that determine when a numeric property is considered "specified".
/// Combine flags with bitwise OR for custom logic.
/// </summary>
/// <remarks>
/// <para>Generated logic based on flags:</para>
/// <list type="bullet">
/// <item><description><see cref="Positive"/> → value &gt; 0</description></item>
/// <item><description><see cref="Zero"/> → value == 0</description></item>
/// <item><description><see cref="Negative"/> → value &lt; 0</description></item>
/// <item><description><see cref="Positive"/> | <see cref="Zero"/> → value &gt;= 0</description></item>
/// <item><description><see cref="Negative"/> | <see cref="Zero"/> → value &lt;= 0</description></item>
/// <item><description><see cref="Positive"/> | <see cref="Negative"/> → value != 0</description></item>
/// <item><description>All three → true (always specified)</description></item>
/// </list>
/// </remarks>
[Flags]
public enum NumericOptions
{
    /// <summary>
    /// Property is specified when value &gt; 0.
    /// </summary>
    Positive = 1,

    /// <summary>
    /// Property is specified when value == 0.
    /// </summary>
    Zero = 2,

    /// <summary>
    /// Property is specified when value &lt; 0.
    /// </summary>
    Negative = 4,
}
