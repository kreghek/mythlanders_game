using System.Collections.Generic;

using Client.Core;

using CombatDicesTeam.Combats;

namespace Client.Assets.Perks;

internal abstract class ImprovedStatBase : IPerk
{
    public abstract void ApplyToStats(ref float maxHitpoints, ref float armorBonus);

    public virtual IReadOnlyCollection<(ICombatantStatType, IStatModifier)> GetStatModifiers()
    {
        return System.Array.Empty<(ICombatantStatType, IStatModifier)>();
    }
}