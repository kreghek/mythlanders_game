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
        
        public IReadOnlyCollection<(UnitStatType, IUnitStatModifier)> GetStatModifiers()
        {
            return new (UnitStatType, IUnitStatModifier)[] { };
        }
    }
    
    internal sealed class StatModifier: IUnitStatModifier
    {
        public int GetBonus(int currentBaseValue)
        {
            return (int)Math.Round(currentBaseValue * 0.5f);
        }
    }
}