using System;
using System.Collections.Generic;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Effects
{
    internal class HealEffect : InstantenousEffectBase
    {
        public override IEnumerable<EffectRule> DispelRules { get; } = new List<EffectRule>();
        public override IEnumerable<EffectRule> ImposeRules { get; } = new List<EffectRule>();
        public override IEnumerable<EffectRule> InfluenceRules { get; } = new List<EffectRule>();

        public int MaxHeal => (int)(Power * PowerMultiplier + ValueRange);

        public int MinHeal => Math.Max((int)(Power * PowerMultiplier - ValueRange), 1);

        public int Power { get; set; }
        public float PowerMultiplier { get; set; }

        public int ValueRange { get; set; }

        protected override void InfluenceAction()
        {
            Target.Unit.TakeHeal(Dice.Roll(MinHeal, MaxHeal));
        }
    }
}