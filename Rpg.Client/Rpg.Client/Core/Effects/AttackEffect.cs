using System;
using System.Collections.Generic;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Effects
{
    internal class AttackEffect : InstantenousEffectBase
    {
        public float PowerMultiplier { get; set; }

        public int ValueRange { get; set; }

        public int Power { get; set; }

        public override IEnumerable<EffectRule> DispelRules { get; } = new List<EffectRule>();
        public override IEnumerable<EffectRule> ImposeRules { get; } = new List<EffectRule>();
        public override IEnumerable<EffectRule> InfluenceRules { get; }= new List<EffectRule>();

        protected override void InfluenceAction()
        {
            var min = Math.Max((int)(Power * PowerMultiplier - ValueRange), 1);
            Target.Unit.TakeDamage(Dice.Roll(min, (int)(Power * PowerMultiplier + ValueRange)));
        }
    }
}