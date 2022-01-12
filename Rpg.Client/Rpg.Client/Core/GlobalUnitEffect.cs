namespace Rpg.Client.Core
{
    internal sealed class GlobalUnitEffect
    {
        public GlobalUnitEffect(IGlobeEvent source)
        {
            Source = source;
        }

        public IGlobeEvent Source { get; }
    }
}