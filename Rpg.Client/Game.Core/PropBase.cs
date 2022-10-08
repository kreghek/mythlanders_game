namespace Game.Core
{
    /// <summary>
    /// Base class for all items.
    /// </summary>
    public abstract class PropBase : IProp
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="scheme"> Item schema. </param>
        protected PropBase(IPropScheme scheme)
        {
            Scheme = scheme;
        }

        public override string ToString()
        {
            return $"{Scheme}";
        }

        /// <summary>
        /// Item schema.
        /// </summary>
        public IPropScheme Scheme { get; }
    }
}