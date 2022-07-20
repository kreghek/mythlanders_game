using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal class IncreaseDamagePercentEffect : ModifiersEffect
    {
        public IncreaseDamagePercentEffect(ICombatUnit actor, IEffectLifetime effectLifetime, float multiplier) : base(
            actor, effectLifetime)
        {
            Modifiers = new List<ModifierBase>
            {
                new GivenDamagePercentageModifier
                {
                    Multiplier = multiplier
                }
            };
            Multiplier = multiplier;
        }

        public float Multiplier { get; }
        protected override IEnumerable<ModifierBase> Modifiers { get; }
    }
}