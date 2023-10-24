using Microsoft.Xna.Framework;

namespace GameClient.Engine.CombatVisualEffects;

/// <summary>
/// Manager to play and update visual effects in combat scene.
/// </summary>
public interface ICombatVisualEffectManager
{

    /// <summary>
    /// Current visual effects.
    /// </summary>
    IReadOnlyCollection<ICombatVisualEffect> Effects { get; }

    /// <summary>
    /// Add new cisual effect.
    /// </summary>
    /// <param name="combatVisualEffect">Effect to add.</param>
    void AddEffect(ICombatVisualEffect combatVisualEffect);

    /// <summary>
    /// Update states of visual effects.
    /// </summary>
    /// <param name="gameTime">Game time to update effects.</param>
    void Update(GameTime gameTime);
}