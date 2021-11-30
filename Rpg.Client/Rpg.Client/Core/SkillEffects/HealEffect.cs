using System;
using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.SkillEffects
{
    internal class HealEffect : InstantenousEffectBase
    {
        public CombatUnit Actor { get; set; }
        public override IEnumerable<EffectRule> DispelRules { get; } = new List<EffectRule>();
        public override IEnumerable<EffectRule> ImposeRules { get; } = new List<EffectRule>();
        public override IEnumerable<EffectRule> InfluenceRules { get; } = new List<EffectRule>();

        public float PowerMultiplier { get; init; }

        public float Scatter { get; init; } = 0.1f;

        public int SourceSupport { get; set; }

        public MinMax<int> CalculateHeal()
        {
            var absoluteSupport = SourceSupport * PowerMultiplier;
            var min = absoluteSupport - Scatter * absoluteSupport;
            var max = absoluteSupport + Scatter * absoluteSupport;

            if (Target != null)
            {
                min = Combat.ModifiersProcessor.Modify(Target, min, ModifierType.TakenHeal);
                max = Combat.ModifiersProcessor.Modify(Target, max, ModifierType.TakenHeal);
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
            var heal = CalculateHeal();
            var rolledHeal = Combat.Dice.Roll(heal.Min, heal.Max);
            Target.Unit.RestoreHitPoints(rolledHeal);
        }
    }
}