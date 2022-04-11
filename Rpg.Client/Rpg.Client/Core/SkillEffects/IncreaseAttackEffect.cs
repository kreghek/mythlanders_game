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

        public int Bonus { get; }

        public override IEnumerable<ModifierBase> Modifiers { get; }
        public override void MergeWithBase(EffectBase testedEffect)
        {
            throw new System.NotImplementedException();
        }
    }
}