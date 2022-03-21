using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal class EffectCreator
    {
        private readonly Func<CombatUnit, CombatSkillEnv, EffectBase> _factory;

        public EffectCreator(Func<CombatUnit, CombatSkillEnv, EffectBase> factory)
        {
            _factory = factory;
        }

        public EffectBase Create(CombatUnit actor, CombatSkillEnv env, Combat combat)
        {
            var effect = _factory(actor, env);
            effect.Combat = combat;

            return effect;
        }

        public EffectBase Create(CombatUnit actor, CombatSkillEnv env)
        {
            var effect = _factory(actor, env);

            return effect;
        }
    }
}