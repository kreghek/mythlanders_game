using System.Diagnostics.CodeAnalysis;

namespace Core
{
    /// <summary>
    /// Arguments for inventory events.
    /// </summary>
    public class PropStoreEventArgs
    {
        [ExcludeFromCodeCoverage]
        public PropStoreEventArgs(IEnumerable<IProp> props)
        {
            Props = props.ToArray();
        }

        [ExcludeFromCodeCoverage]
        public PropStoreEventArgs(params IProp[] props)
        {
            Props = props;
        }

        public IProp[] Props { get; }
    }
}