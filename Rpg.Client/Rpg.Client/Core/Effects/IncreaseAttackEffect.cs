using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.Effects
{
    internal class IncreaseAttackEffect : ModifiersEffect
    {
        public IncreaseAttackEffect(int bonus)
        {
            Modifiers = new List<ModifierBase>
            {
                new GivenDamageAbsoluteModifier
                {
                    DamageBonus = bonus
                }
            };
        }

        public override IEnumerable<ModifierBase> Modifiers { get; }
    }
}