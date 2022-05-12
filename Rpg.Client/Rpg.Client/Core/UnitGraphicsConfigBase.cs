using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal abstract class UnitGraphicsConfigBase
    {
        protected UnitGraphicsConfigBase()
        {
            PredefinedAnimations = new Dictionary<AnimationSid, IAnimationFrameSet>();
        }

        public IDictionary<AnimationSid, IAnimationFrameSet> PredefinedAnimations { get; protected set; }
    }
}