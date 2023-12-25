using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Engine.CombatVisualEffects;

/// <summary>
/// Composite effect to run multiple effects in same time.
/// </summary>
public sealed class ParallelCombatVisualEffect : ICombatVisualEffect
{
    private readonly IReadOnlyCollection<ICombatVisualEffect> _baseEffects;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="baseEffects">Effect to run in same time.</param>
    public ParallelCombatVisualEffect(params ICombatVisualEffect[] baseEffects)
    {
        _baseEffects = baseEffects;
    }

    /// <inheritdoc />
    public bool IsDestroyed => _baseEffects.All(x => x.IsDestroyed);

    /// <inheritdoc />
    public void DrawBack(SpriteBatch spriteBatch)
    {
        foreach (var item in _baseEffects)
        {
            item.DrawBack(spriteBatch);
        }
    }

    /// <inheritdoc />
    public void DrawFront(SpriteBatch spriteBatch)
    {
        foreach (var item in _baseEffects)
        {
            item.DrawFront(spriteBatch);
        }
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        foreach (var item in _baseEffects)
        {
            item.Update(gameTime);
        }
    }
}