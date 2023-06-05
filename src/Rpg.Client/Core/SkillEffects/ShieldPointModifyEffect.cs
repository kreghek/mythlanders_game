using System.Collections.Generic;

namespace Rpg.Client.Core.SkillEffects
{
    internal sealed class ShieldPointModifyEffect : ModifyStatEffectBase
    {
        public ShieldPointModifyEffect(ICombatUnit actor, IEffectLifetime lifetime, float modifier) : base(actor,
            lifetime)
        {
            Modifier = modifier;
        }

        public float Modifier { get; }

        protected override IEnumerable<(UnitStatType, StatModifier)> Modifiers => new (UnitStatType, StatModifier)[]
        {
            new(UnitStatType.ShieldPoints, new StatModifier(Modifier))
        };
    }
}