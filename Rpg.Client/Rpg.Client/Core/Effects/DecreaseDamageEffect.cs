using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.Effects
{
    internal class DecreaseDamageEffect : ModifiersEffect
    {
        private readonly float _multiplier;

        public DecreaseDamageEffect(float multiplier)
        {
            _multiplier = multiplier;
            Modifiers = new List<ModifierBase>
            {
                new GivenDamageModifier
                {
                    DamageMultiplier = _multiplier
                }
            };
        }

        public override IEnumerable<ModifierBase> Modifiers { get; }

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