using System;
using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Effects
{
    internal class AttackEffect : InstantenousEffectBase
    {
        public CombatUnit Actor { get; set; }
        public override IEnumerable<EffectRule> DispelRules { get; } = new List<EffectRule>();
        public override IEnumerable<EffectRule> ImposeRules { get; } = new List<EffectRule>();

        public override IEnumerable<EffectRule> InfluenceRules { get; } = new List<EffectRule>();

        public float DamageMultiplier { get; init; }

        public float Scatter { get; init; } = 0.1f;

        public MinMax<int> CalculateDamage()
        {
            var absoluteDamage = Actor.Unit.Damage * DamageMultiplier;
            var min = absoluteDamage - Scatter * absoluteDamage;
            var max = absoluteDamage + Scatter * absoluteDamage;

            min = Combat.ModifiersProcessor.Modify(Actor, min, ModifierType.GivenDamage);
            max = Combat.ModifiersProcessor.Modify(Actor, max, ModifierType.GivenDamage);

            if (Target is not null)
            {
                min = Combat.ModifiersProcessor.Modify(Target, min, ModifierType.TakenDamage);
                max = Combat.ModifiersProcessor.Modify(Target, max, ModifierType.TakenDamage);
            }

            return new MinMax<int>
            {
                Min = Math.Max((int)min, 1),
                Max = (int)max
            };
        }

        protected override void InfluenceAction()
        {
            var damage = CalculateDamage();
            var rolledDamage = Combat.Dice.Roll(damage.Min, damage.Max);
            Target.Unit.TakeDamage(Actor, rolledDamage);
        }
    }
}