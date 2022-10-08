namespace Game.Core
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
}