﻿using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal class ModifyDamageEffect : ModifiersEffect
    {
        public ModifyDamageEffect(ICombatUnit actor, IEffectLifetime lifetime, int bonus) : base(actor, lifetime)
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