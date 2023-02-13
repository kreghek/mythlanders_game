﻿using System.Collections.Generic;

using Client.Core;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Perks
{
    internal abstract class ImprovedStatBase : IPerk
    {
        public abstract void ApplyToStats(ref float maxHitpoints, ref float armorBonus);

        public virtual IReadOnlyCollection<(UnitStatType, IUnitStatModifier)> GetStatModifiers()
        {
            return new (UnitStatType, IUnitStatModifier)[] { };
        }
    }
}