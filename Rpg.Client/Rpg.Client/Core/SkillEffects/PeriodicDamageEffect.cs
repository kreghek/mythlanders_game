using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal class PeriodicDamageEffect : PeriodicEffectBase
    {
        public CombatUnit Actor { get; set; }
        public float PowerMultiplier { get; init; }

        public float Scatter { get; init; } = 0.1f;
        public int SourceDamage { get; set; }

        public MinMax<int> CalculateDamage()
        {
            var absoluteDamage = SourceDamage * PowerMultiplier;
            var min = absoluteDamage - Scatter * absoluteDamage;
            var max = absoluteDamage + Scatter * absoluteDamage;

            // if (Combat is not null)
            // {
            //     if (Target != null)
            //     {
            //         min = Combat.ModifiersProcessor.Modify(Target, min, ModifierType.TakenHeal);
            //         max = Combat.ModifiersProcessor.Modify(Target, max, ModifierType.TakenHeal);
            //     }
            // }

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
            var heal = CalculateDamage();
            var rolledHeal = Combat.Dice.Roll(heal.Min, heal.Max);
            Target.Unit.TakeDamage(Actor, rolledHeal);

            base.InfluenceAction();
        }
    }
}