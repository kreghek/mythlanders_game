namespace Core.Props
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

        /// <summary>
        /// Item schema.
        /// </summary>
        public IPropScheme Scheme { get; }

        public override string ToString()
        {
            return $"{Scheme}";
        }
    }
}