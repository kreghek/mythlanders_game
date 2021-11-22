using System;

namespace Rpg.Client.Core.Perks
{
    internal sealed class ImprovedHitPoints : ImprovedStat
    {
        private const float HITPOINTS_BONUS = 2f;

        public override void ApplyToStats(ref int maxHitpoints, ref float armorBonus)
        {
            maxHitpoints = (int)Math.Round(maxHitpoints * HITPOINTS_BONUS);
        }
    }
}