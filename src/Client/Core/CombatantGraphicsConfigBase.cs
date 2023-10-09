using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Client.Core;

internal abstract class CombatantGraphicsConfigBase
{
    public Vector2 InteractionPoint { get; protected set; } = Vector2.UnitY * 32;
    public Vector2 LaunchPoint { get; protected set; } = new Vector2(64, 64);

    public int MeleeHitXOffset { get; protected set; } = 64;
    public Vector2 ShadowOrigin { get; protected set; } = new Vector2(0, 0);
    public Vector2 StatsPanelOrigin { get; protected set; } = new Vector2(0, 64 + 4);

    public abstract string ThumbnailPath { get; }

    public abstract IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations();
}