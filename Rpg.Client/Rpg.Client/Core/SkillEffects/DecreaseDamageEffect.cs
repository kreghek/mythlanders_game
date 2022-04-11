using System;
using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal class DecreaseDamageEffect : ModifiersEffect
    {
        public DecreaseDamageEffect(float multiplier)
        {
            Multiplier = multiplier;

            Modifiers = new List<ModifierBase>
            {
                new GivenDamageModifier
                {
                    DamageMultiplier = Multiplier
                }
            };
        }

        public override IEnumerable<ModifierBase> Modifiers { get; }
        public float Multiplier { get; }

        protected override void AfterDispel()
        {
            Target.ChangeState(CombatUnitState.Idle);
        }

        protected override void AfterImpose()
        {
            Target.ChangeState(CombatUnitState.Defense);
        }

        public override void MergeWithBase(EffectBase testedEffect)
        {
            throw new NotImplementedException();
        }
    }
}