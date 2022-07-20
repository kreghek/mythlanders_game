using System.Collections.Generic;

namespace Rpg.Client.Core.SkillEffects
{
    internal sealed class HitPointModifyEffect : ModifyStatEffectBase
    {
        private readonly float _modifier;

        public HitPointModifyEffect(ICombatUnit actor, IEffectLifetime lifetime, float modifier) : base(actor,
            lifetime)
        {
            _modifier = modifier;
        }

        public float Modifier => _modifier;

        protected override IEnumerable<(UnitStatType, StatModifier)> Modifiers => new (UnitStatType, StatModifier)[]
        {
            new(UnitStatType.HitPoints, new StatModifier(Modifier))
        };
    }
}