using System.Collections.Generic;

using Client.Core;

using Core.Combats;

namespace Client.Assets.Perks;

internal abstract class ImprovedStatBase : IPerk
{
    public abstract void ApplyToStats(ref float maxHitpoints, ref float armorBonus);

    public virtual IReadOnlyCollection<(ICombatantStatType, IUnitStatModifier)> GetStatModifiers()
    {
        return new (ICombatantStatType, IUnitStatModifier)[] { };
    }
}