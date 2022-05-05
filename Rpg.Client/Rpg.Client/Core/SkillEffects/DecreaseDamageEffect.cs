using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal class DecreaseDamageEffect : ModifiersEffect
    {
        public DecreaseDamageEffect(ICombatUnit actor, int duration, float multiplier) : base(actor, duration)
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

        public float Multiplier { get; }

        protected override IEnumerable<ModifierBase> Modifiers { get; }

        protected override void AfterDispel()
        {
            Target.ChangeState(CombatUnitState.Idle);
        }
    }
}