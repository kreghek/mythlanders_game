using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal class IncreaseDamagePercentEffect : ModifiersEffect
    {
        public IncreaseDamagePercentEffect(ICombatUnit actor, int duration, float multiplier) : this(actor, new DurationEffectLifetime(duration), multiplier)
        {
        }

        public IncreaseDamagePercentEffect(ICombatUnit actor, IEffectLifetime effectLifetime, float multiplier): base(actor, effectLifetime)
        {
            Modifiers = new List<ModifierBase>
            {
                new GivenDamagePercentageModifier
                {
                    Multiplier = multiplier
                }
            };
        }

        protected override IEnumerable<ModifierBase> Modifiers { get; }
    }
}