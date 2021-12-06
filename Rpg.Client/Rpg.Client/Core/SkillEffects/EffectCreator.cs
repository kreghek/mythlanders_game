using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal class EffectCreator
    {
        private readonly Func<CombatUnit, EffectBase> _factory;

        public EffectCreator(Func<CombatUnit, EffectBase> factory)
        {
            _factory = factory;
        }

        public EffectBase Create(CombatUnit actor, Combat combat)
        {
            var effect = _factory(actor);
            effect.Combat = combat;

            return effect;
        }

        public EffectBase Create(CombatUnit actor)
        {
            var effect = _factory(actor);

            return effect;
        }
    }
}