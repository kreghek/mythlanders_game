using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Assets.ActorVisualizationStates.Primitives;

/// <summary>
/// Visual effect in combat.
/// </summary>
interface ICombatVisualEffect
{
    /// <summary>
    /// Update effect state.
    /// </summary>
    void Update(GameTime gameTime);

    /// <summary>
    /// Draw background part of effect.
    /// </summary>
    void DrawBack(SpriteBatch spriteBatch);

    /// <summary>
    /// Draw front part of effect.
    /// </summary>
    void DrawFront(SpriteBatch spriteBatch);

    /// <summary>
    /// Effect is destroyed. It should be removed from render and updating.
    /// </summary>
    bool IsDestroyed { get; }
}