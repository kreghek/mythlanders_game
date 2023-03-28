namespace Core.Props
{
    /// <summary>
    /// Item schema.
    /// </summary>
    public interface IPropScheme : IScheme
    {
        /// <summary>
        /// Item tags. Used for description and for some rules.
        /// </summary>
        string?[]? Tags { get; }
    }

    public sealed class PropScheme : IPropScheme
    {
        public string?[]? Tags { get; init; }

        public string? Sid { get; set; }
    }
}