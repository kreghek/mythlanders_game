using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.GameScreens;

using GameClient.Engine.Animations;
using GameClient.Engine.CombatVisualEffects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Client.Core;

internal abstract class CombatantGraphicsConfigBase
{
    public Vector2 InteractionPoint { get; protected set; } = Vector2.UnitY * 32;
    public Vector2 LaunchPoint { get; protected set; } = new Vector2(64, 64);
    public int MeleeHitXOffset { get; protected set; } = 64;

    /// <summary>
    /// Sprite origin in pixels.
    /// </summary>
    public Vector2 Origin { get; protected set; } = new Vector2(60, 110);

    /// <summary>
    /// Used for flying or exploitable combatants.
    /// Do not shadow on combatant corpse.
    /// </summary>
    public bool RemoveShadowOnDeath { get; protected set; } = false;

    public Vector2 StatsPanelOrigin { get; protected set; } = new Vector2(0, 64 + 4);

    public abstract string ThumbnailPath { get; }

    public abstract IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations();

    public virtual IAnimationFrameSet GetDeathAnimation(GameObjectContentStorage gameObjectContentStorage,
        ICombatVisualEffectManager combatVisualEffectManager,
        AudioSettings audioSettings,
        Vector2 position)
    {
        return GetPredefinedAnimations()[PredefinedAnimationSid.Death];
    }

    public virtual void LoadContent(ContentManager contentManager) { }
}