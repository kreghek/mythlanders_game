using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Core
{
    internal abstract class UnitGraphicsConfigBase
    {
        public Vector2 InteractionPoint { get; protected set; } = Vector2.UnitY * 64;
        public Vector2 StatsPanelOrigin { get; protected set; } = new Vector2(0, 64 + 4);

        public abstract IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations();
    }
}