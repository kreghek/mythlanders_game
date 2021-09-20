using System;
using System.Collections.Generic;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Effects
{
    internal class AttackEffect : InstantenousEffectBase
    {
        public override IEnumerable<EffectRule> DispelRules { get; } = new List<EffectRule>();
        public override IEnumerable<EffectRule> ImposeRules { get; } = new List<EffectRule>();
        public override IEnumerable<EffectRule> InfluenceRules { get; } = new List<EffectRule>();

        public int Power { get; set; }
        public float PowerMultiplier { get; set; }

        public int ValueRange { get; set; }

        public int MinDamage => Math.Max((int)(Power * PowerMultiplier - ValueRange), 1);
        public int MaxDamage => (int)(Power * PowerMultiplier + ValueRange);

        protected override void InfluenceAction()
        {
            var rolledDamage = Dice.Roll(MinDamage, MaxDamage);
            Target.Unit.TakeDamage(rolledDamage);
        }
    }
}