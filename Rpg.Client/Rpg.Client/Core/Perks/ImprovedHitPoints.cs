using System;

namespace Rpg.Client.Core.Perks
{
    internal sealed class ImprovedHitPoints : ImprovedStatBase
    {
        private const float HITPOINTS_BONUS = 3.5f;

        public override void ApplyToStats(ref int maxHitpoints, ref float armorBonus)
        {
            maxHitpoints = (int)Math.Round(maxHitpoints * HITPOINTS_BONUS);
        }
    }
}