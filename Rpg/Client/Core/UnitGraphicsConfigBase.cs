using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Client.Core;

internal abstract class UnitGraphicsConfigBase
{
    public Vector2 InteractionPoint { get; protected set; } = Vector2.UnitY * 32;
    public Vector2 LaunchPoint { get; protected set; } = new Vector2(64, 64);
    public Vector2 ShadowOrigin { get; protected set; } = new Vector2(0, 0);
    public Vector2 StatsPanelOrigin { get; protected set; } = new Vector2(0, 64 + 4);

    public int MeleeHitXOffset { get; protected set; } = 64;

    public abstract IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations();

    public virtual string ThumbnalPath => Path.Combine("Sprites", "GameObjects", "Characters", "Heroes",
    GetType().Name[..^14], "Thumbnail");
}