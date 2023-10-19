using System;
using System.Collections.Generic;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.Assets.Perks;

internal sealed class ImprovedHitPoints : ImprovedStatBase, IStatModifierSource
{
    private const float HITPOINTS_BONUS = 1.5f;

    public override void ApplyToStats(ref float maxHitpoints, ref float armorBonus)
    {
        maxHitpoints = (float)Math.Round(maxHitpoints * HITPOINTS_BONUS);
    }

    public override IReadOnlyCollection<(ICombatantStatType, IStatModifier)> GetStatModifiers()
    {
        return new (ICombatantStatType, IStatModifier)[]
        {
            new(CombatantStatTypes.HitPoints, new StatModifier(1, this))
        };
    }
}