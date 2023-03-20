using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Core
{
    internal abstract class UnitGraphicsConfigBase
    {
        public Vector2 InteractionPoint { get; init; }
        public abstract IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations();
    }
}