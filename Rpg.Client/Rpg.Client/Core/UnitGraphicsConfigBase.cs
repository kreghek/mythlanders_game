using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal abstract class UnitGraphicsConfigBase
    {
        protected UnitGraphicsConfigBase()
        {
            Animations = new Dictionary<AnimationSid, AnimationInfo>();
        }

        public Dictionary<AnimationSid, AnimationInfo> Animations { get; protected set; }
    }
}