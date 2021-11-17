using System;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.Effects
{
    internal class PeriodicHealEffect : PeriodicEffectBase
    {
        public int SourceSupport { get; set; }
        public float PowerMultiplier { get; init; }

        public float Scatter { get; init; } = 0.1f;

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

            return new MinMax<int>
            {
                Min = Math.Max((int)min, 1),
                Max = (int)max
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