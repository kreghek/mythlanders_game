using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal class EffectCreator
    {
        private readonly Func<ICombatUnit, EffectBase> _factory;

        public EffectCreator(Func<ICombatUnit, EffectBase> factory)
        {
            _factory = factory;
        }

        public EffectBase Create(ICombatUnit actor, ICombat combat)
        {
            var effect = _factory(actor);
            effect.Combat = combat;

            return effect;
        }

        public EffectBase Create(ICombatUnit actor)
        {
            var effect = _factory(actor);

            return effect;
        }
    }
}