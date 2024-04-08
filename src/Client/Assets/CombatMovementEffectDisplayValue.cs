namespace Client.Assets;

/// <summary>
/// Key value with meta data.
/// </summary>
/// <param name="Tag">Tag in the description to represent key value.</param>
/// <param name="Value">Value itself.</param>
/// <param name="Template">Template to display the value.</param>
public sealed record DescriptionKeyValue(
    string Tag,
    int Value,
    DescriptionKeyValueTemplate Template);