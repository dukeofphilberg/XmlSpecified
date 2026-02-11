namespace XmlSpecified;

/// <summary>
/// Determines when a boolean property is considered "specified".
/// </summary>
public enum BoolOptions
{
    /// <summary>
    /// Property is specified when value == true.
    /// </summary>
    True = 0,

    /// <summary>
    /// Property is specified when value == false.
    /// </summary>
    False = 1,
}
