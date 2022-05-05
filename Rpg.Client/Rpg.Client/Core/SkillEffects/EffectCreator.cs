using System;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.SkillEffects
{
    internal class EffectCreator
    {
        private readonly Func<ICombatUnit, EffectBase> _factory;

        public EffectCreator(Func<ICombatUnit, EffectBase> factory)
        {
            _factory = factory;
        }

        public EffectBase Create(ICombatUnit actor, ICombat combat, ISkill sourceSkill)
        {
            var effect = _factory(actor);

            var context = new CombatEffectContext(combat, sourceSkill);

            effect.CombatContext = context;

            return effect;
        }

        public EffectBase Create(ICombatUnit actor)
        {
            var effect = _factory(actor);

            return effect;
        }
    }
}