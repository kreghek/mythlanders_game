using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.Effects
{
    internal class IncreaseAttackEffect : ModifiersEffect
    {
        private readonly float _multiplier;

        public IncreaseAttackEffect(float multiplier)
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
    }
}