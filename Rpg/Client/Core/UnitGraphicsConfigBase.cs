using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal abstract class UnitGraphicsConfigBase
    {
        public abstract IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations();
    }
}