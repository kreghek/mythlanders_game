﻿using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal class IncreaseAttackEffect : ModifiersEffect
    {
        public IncreaseAttackEffect(ICombatUnit actor, int duration, int bonus) : base(actor, duration)
        {
            Modifiers = new List<ModifierBase>
            {
                new GivenDamageAbsoluteModifier
                {
                    DamageBonus = bonus
                }
            };
            Bonus = bonus;
        }

        public int Bonus { get; }

        protected override IEnumerable<ModifierBase> Modifiers { get; }
    }
}