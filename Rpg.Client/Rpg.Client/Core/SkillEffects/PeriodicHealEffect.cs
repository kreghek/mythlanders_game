using System;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal class PeriodicHealEffect : PeriodicEffectBase
    {
        public PeriodicHealEffect(ICombatUnit actor, int startDuration) : this(actor, new DurationEffectLifetime(startDuration))
        {
        }

        public PeriodicHealEffect(ICombatUnit actor, IEffectLifetime effectLifetime) : base(actor, effectLifetime)
        {
            SourceSupport = actor.Unit.Support;
        }


        public float PowerMultiplier { get; init; } = 1f;

        public float Scatter { get; init; } = 0.1f;
        public int SourceSupport { get; init; }

        public MinMax<int> CalculateHeal()
        {
            var absoluteSupport = SourceSupport * PowerMultiplier;
            var min = absoluteSupport - Scatter * absoluteSupport;
            var max = absoluteSupport + Scatter * absoluteSupport;

            if (CombatContext is not null)
            {
                if (Target != null)
                {
                    min = CombatContext.Combat.ModifiersProcessor.Modify(Target, min, ModifierType.TakenHeal);
                    max = CombatContext.Combat.ModifiersProcessor.Modify(Target, max, ModifierType.TakenHeal);
                }
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
            if (CombatContext is null)
            {
                throw new InvalidOperationException();
            }

            if (Target is null)
            {
                throw new InvalidOperationException();
            }

            var heal = CalculateHeal();
            var rolledHeal = CombatContext.Combat.Dice.Roll(heal.Min, heal.Max);
            Target.Unit.RestoreHitPoints(rolledHeal);

            base.InfluenceAction();
        }
    }
}