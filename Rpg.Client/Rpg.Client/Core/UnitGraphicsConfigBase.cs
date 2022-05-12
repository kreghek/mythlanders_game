using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal abstract class UnitGraphicsConfigBase
    {
        protected UnitGraphicsConfigBase()
        {
            PredefinedAnimations = new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>();
        }

        public IDictionary<PredefinedAnimationSid, IAnimationFrameSet> PredefinedAnimations { get; protected set; }
    }
}