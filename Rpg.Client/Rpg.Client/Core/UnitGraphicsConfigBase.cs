using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal abstract class UnitGraphicsConfigBase
    {
        protected UnitGraphicsConfigBase()
        {
            PredefinedAnimations = new Dictionary<AnimationSid, AnimationFrameSet>();
        }

        public IDictionary<AnimationSid, AnimationFrameSet> PredefinedAnimations { get; protected set; }
    }
}