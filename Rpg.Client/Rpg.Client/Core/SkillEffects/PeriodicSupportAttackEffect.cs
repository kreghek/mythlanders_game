using System;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal class PeriodicSupportAttackEffect : PeriodicEffectBase
    {
        public ICombatUnit Actor { get; set; }
        public float PowerMultiplier { get; init; }

        public float Scatter { get; init; } = 0.1f;

        public int SourceSupport { get; set; }

        public MinMax<int> CalculateRoundDamage()
        {
            var absoluteSupport = SourceSupport * PowerMultiplier;
            var min = absoluteSupport - Scatter * absoluteSupport;
            var max = absoluteSupport + Scatter * absoluteSupport;

            if (Combat is not null)
            {
                if (Target != null)
                {
                    min = Combat.ModifiersProcessor.Modify(Target, min, ModifierType.TakenDamage);
                    max = Combat.ModifiersProcessor.Modify(Target, max, ModifierType.TakenDamage);
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

        public override bool CanBeMerged(EffectBase testedEffect)
        {
            if (testedEffect is PeriodicSupportAttackEffect periodicDamageEffect)
            {
                if (Actor == periodicDamageEffect.Actor)
                {
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public override void MergeWithBase(EffectBase testedEffect)
        {
            if (testedEffect is PeriodicSupportAttackEffect periodicDamageEffect)
            {
                if (Actor == periodicDamageEffect.Actor)
                {
                    periodicDamageEffect.Duration += Duration;
                    periodicDamageEffect.SourceSupport = SourceSupport;
                }
                else
                {
                    throw new InvalidOperationException("The tested effect has not same author.");
                }
            }
            else
            {
                throw new InvalidOperationException("Base is not same type");
            }
        }

        protected override void InfluenceAction()
        {
            var damageRange = CalculateRoundDamage();
            var rolledDamage = Combat.Dice.Roll(damageRange.Min, damageRange.Max);
            Target.Unit.TakeDamage(Actor, rolledDamage);

            base.InfluenceAction();
        }
    }
}