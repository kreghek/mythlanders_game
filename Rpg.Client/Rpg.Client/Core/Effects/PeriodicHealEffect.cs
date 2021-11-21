using System;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.Effects
{
    internal class PeriodicHealEffect : PeriodicEffectBase
    {
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

            base.InfluenceAction();
        }
    }
}