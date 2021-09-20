using System;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Effects
{
    internal class EffectCreator
    {
        private readonly Func<CombatUnit, EffectBase> _factory;

        public EffectCreator(Func<CombatUnit, EffectBase> factory)
        {
            _factory = factory;
        }

        public EffectBase Create(CombatUnit actor, EffectProcessor effectProcessor, IDice dice, ActiveCombat combat)
        {
            var effect = _factory(actor);
            effect.EffectProsessor = effectProcessor;
            effect.Dice = dice;
            effect.Combat = combat;

            return effect;
        }

        //TODO Looks like design smell.
        public EffectBase CreateToDisplay(CombatUnit actor)
        {
            var effect = _factory(actor);
            return effect;
        }
    }
}