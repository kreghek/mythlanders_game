using System;
using System.Collections.Generic;
using System.Diagnostics;

using Rpg.Client.Core.Modifiers;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.SkillEffects
{
    internal class LifeDrawEffect : InstantaneousEffectBase
    {
        public ICombatUnit Actor { get; set; }

        public float DamageMultiplier { get; init; }
        public override IEnumerable<EffectRule> DispelRules { get; } = new List<EffectRule>();
        public override IEnumerable<EffectRule> ImposeRules { get; } = new List<EffectRule>();

        public override IEnumerable<EffectRule> InfluenceRules { get; } = new List<EffectRule>();

        public float Scatter { get; init; } = 0.1f;

        public MinMax<int> CalculateDamage()
        {
            var absoluteDamage = Actor.Unit.Damage * DamageMultiplier;
            var min = absoluteDamage - Scatter * absoluteDamage;
            var max = absoluteDamage + Scatter * absoluteDamage;

            min = CombatContext.Combat.ModifiersProcessor.Modify(Actor, min, ModifierType.GivenDamage);
            max = CombatContext.Combat.ModifiersProcessor.Modify(Actor, max, ModifierType.GivenDamage);

            if (Target is not null)
            {
                min = CombatContext.Combat.ModifiersProcessor.Modify(Target, min, ModifierType.TakenDamage);
                max = CombatContext.Combat.ModifiersProcessor.Modify(Target, max, ModifierType.TakenDamage);
            }

            var absoluteMin = (int)Math.Round(min, MidpointRounding.AwayFromZero);
            var absoluteMax = (int)Math.Round(max, MidpointRounding.AwayFromZero);

            return new MinMax<int>
            {
                Min = Math.Max(absoluteMin, 0),
                Max = Math.Max(absoluteMin, absoluteMax)
            };
        }

        protected override void InfluenceAction()
        {
            var damage = CalculateDamage();
            var rolledDamage = CombatContext.Combat.Dice.Roll(damage.Min, damage.Max);

            var accumulatedDamage = rolledDamage;
            foreach (var perk in Actor.Unit.Perks)
            {
                var modifiedDamage = perk.ModifyDamage(accumulatedDamage, CombatContext.Combat.Dice);
                accumulatedDamage = modifiedDamage;
            }

            Debug.Assert(Target is not null);
            var hpToSteal = Target.TakeDamage(Actor, accumulatedDamage);

            Debug.Assert(hpToSteal.ValueFinal is not null);

            var accumulatedhpToSteal = hpToSteal.ValueFinal.Value;
            foreach (var perk in Actor.Unit.Perks)
            {
                var modifiedHeal = perk.ModifyHeal(accumulatedhpToSteal, CombatContext.Combat.Dice);
                accumulatedhpToSteal = modifiedHeal;
            }

            Actor.RestoreHitPoints(accumulatedhpToSteal);
        }
    }
}