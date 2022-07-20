using System.Collections.Generic;

namespace Rpg.Client.Core.SkillEffects
{
    internal sealed class ShieldPointModifyEffect : ModifyStatEffectBase
    {
        private readonly float _modifier;

        public ShieldPointModifyEffect(ICombatUnit actor, IEffectLifetime lifetime, float modifier) : base(actor,
            lifetime)
        {
            _modifier = modifier;
        }

        public float Modifier => _modifier;

        protected override IEnumerable<(UnitStatType, StatModifier)> Modifiers => new (UnitStatType, StatModifier)[]
        {
            new(UnitStatType.ShieldPoints, new StatModifier(Modifier))
        };
    }
}