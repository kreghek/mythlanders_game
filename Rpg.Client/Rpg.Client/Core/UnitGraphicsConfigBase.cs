using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal abstract class UnitGraphicsConfigBase
    {
        public const string DEFAULT_ANIMATION_SID = "Idle";
        public Dictionary<string, AnimationInfo> Animations { get; protected set; }
    }
}