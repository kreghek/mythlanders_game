using System;
using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Perks
{
    internal sealed class ImprovedHitPoints : ImprovedStatBase
    {
        private const float HITPOINTS_BONUS = 1.5f;

        public override void ApplyToStats(ref float maxHitpoints, ref float armorBonus)
        {
            maxHitpoints = (float)Math.Round(maxHitpoints * HITPOINTS_BONUS);
        }

        public override IReadOnlyCollection<(UnitStatType, IUnitStatModifier)> GetStatModifiers()
        {
            return new (UnitStatType, IUnitStatModifier)[] {
                new (UnitStatType.HitPoints, new StatModifier(0.5f))
            };
        }
    }
}