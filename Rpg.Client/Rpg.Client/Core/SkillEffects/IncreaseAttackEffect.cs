using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
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
            Bonus = bonus;
        }

        public override IEnumerable<ModifierBase> Modifiers { get; }
        public int Bonus { get; }
    }
}