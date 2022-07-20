using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class GlobalUnitEffect: IEffectSource
    {
        public GlobalUnitEffect(IGlobeEvent source)
        {
            Source = source;
        }

        public IGlobeEvent Source { get; }
    }
}