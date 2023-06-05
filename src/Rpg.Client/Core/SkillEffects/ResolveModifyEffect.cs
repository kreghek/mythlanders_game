using System.Collections.Generic;

namespace Rpg.Client.Core.SkillEffects
{
    internal sealed class ResolveModifyEffect : ModifyStatEffectBase
    {
        private readonly float _modifier;

        public ResolveModifyEffect(ICombatUnit actor, IEffectLifetime lifetime, float modifier) : base(actor,
            lifetime)
        {
            _modifier = modifier;
        }

        protected override IEnumerable<(UnitStatType, StatModifier)> Modifiers => new (UnitStatType, StatModifier)[]
        {
            new(UnitStatType.Resolve, new StatModifier(_modifier))
        };
    }
}