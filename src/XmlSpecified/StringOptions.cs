namespace XmlSpecified;

/// <summary>
/// Determines when a string property is considered "specified".
/// </summary>
public enum StringOptions
{
    /// <summary>
    /// Property is specified when !string.IsNullOrWhiteSpace(value).
    /// </summary>
    NonWhitespace = 0,

    /// <summary>
    /// Property is specified when !string.IsNullOrEmpty(value).
    /// </summary>
    NonEmpty = 1,

    /// <summary>
    /// Property is specified when value != null.
    /// </summary>
    NonNull = 2,
}
