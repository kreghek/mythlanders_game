using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal class DecreaseDamageEffect : ModifiersEffect
    {
        public DecreaseDamageEffect(float multiplier)
        {
            Modifiers = new List<ModifierBase>
            {
                new GivenDamageModifier
                {
                    DamageMultiplier = multiplier
                }
            };
            Multiplier = multiplier;
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
    }
}