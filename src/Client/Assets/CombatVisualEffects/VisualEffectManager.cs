using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Client.Assets.CombatVisualEffects;

/// <summary>
/// Visual effects in the combat.
/// </summary>
internal sealed class VisualEffectManager
{
    private readonly IList<ICombatVisualEffect> _list = new List<ICombatVisualEffect>();

    /// <summary>
    /// Current effects snapshot.
    /// </summary>
    public IReadOnlyCollection<ICombatVisualEffect> Effects => _list.ToArray();

    /// <summary>
    /// Add effect to combat.
    /// </summary>
    public void AddEffect(ICombatVisualEffect combatVisualEffect)
    {
        _list.Add(combatVisualEffect);
    }

    /// <summary>
    /// Update current effects.
    /// </summary>
    public void Update(GameTime gameTime)
    {
        foreach (var effect in Effects)
        {
            effect.Update(gameTime);

            if (effect.IsDestroyed)
            {
                _list.Remove(effect);
            }
        }
    }
}