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
        //public int MaxDamage => (int)(Power * PowerMultiplier + ValueRange);

        //public int MinDamage => Math.Max((int)(Power * PowerMultiplier - ValueRange), 1);

        //public int Power { get; set; }
        public float PowerMultiplier { get; set; }

        public int ValueRange { get; set; }

        public MinMax<int> CalculateDamage()
        {
            if (Actor is null)
            {
                return new MinMax<int>();
            }

            var min = Actor.Unit.Power * PowerMultiplier - ValueRange;
            var max = Actor.Unit.Power * PowerMultiplier + ValueRange;

            min = Combat.ModifiersProcessor.Modify(Actor, min, ModifierType.GivenDamage);
            max = Combat.ModifiersProcessor.Modify(Actor, max, ModifierType.GivenDamage);

            if (Target != null)
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