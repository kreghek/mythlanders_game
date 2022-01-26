using System;

namespace Rpg.Client.Assets.Perks
{
    internal sealed class ImprovedHitPoints : ImprovedStatBase
    {
        private const float HITPOINTS_BONUS = 1.5f;

        public override void ApplyToStats(ref float maxHitpoints, ref float armorBonus)
        {
            maxHitpoints = (float)Math.Round(maxHitpoints * HITPOINTS_BONUS);
        }
    }
}