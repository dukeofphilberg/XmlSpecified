namespace XmlSpecified;

/// <summary>
/// Determines when a collection property is considered "specified".
/// </summary>
public enum CollectionOptions
{
    /// <summary>
    /// Property is specified when value != null and has elements (Length/Count &gt; 0).
    /// </summary>
    NonEmpty = 0,

    /// <summary>
    /// Property is specified when value != null (empty collections are allowed).
    /// </summary>
    NonNull = 1,
}
