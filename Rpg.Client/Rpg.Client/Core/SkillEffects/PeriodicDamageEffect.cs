using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal class PeriodicDamageEffect : PeriodicEffectBase
    {
        public PeriodicDamageEffect(ICombatUnit actor, int startDuration) : base(actor, startDuration)
        {
            SourceDamage = actor.Unit.Damage;
        }

        public float PowerMultiplier { get; init; } = 1f;

        public float Scatter { get; init; } = 0.1f;

        public int SourceDamage { get; init; }

        public MinMax<int> CalculateDamage()
        {
            var absoluteDamage = SourceDamage * PowerMultiplier;
            var min = absoluteDamage - Scatter * absoluteDamage;
            var max = absoluteDamage + Scatter * absoluteDamage;

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
            var damageRange = CalculateDamage();
            var rolledDamage = CombatContext.Combat.Dice.Roll(damageRange.Min, damageRange.Max);
            Target.Unit.TakeDamage(Actor, rolledDamage);

            base.InfluenceAction();
        }
    }
}