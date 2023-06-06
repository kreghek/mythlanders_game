using System;

namespace Client.Assets.Perks;

internal sealed class BigMonster : ImprovedStatBase
{
    private const float HITPOINTS_BONUS = 1.5f;
    private const float ARMOR_BONUS = 2f;

    public override void ApplyToStats(ref float maxHitpoints, ref float armorBonus)
    {
        maxHitpoints = (float)Math.Round(maxHitpoints * HITPOINTS_BONUS);
        armorBonus = ARMOR_BONUS;
    }
}