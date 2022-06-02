using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal class StunEffect : PeriodicEffectBase
    {
        public StunEffect(ICombatUnit actor, IEffectLifetime lifetime) : base(actor, lifetime)
        {
        }

        public StunEffect(ICombatUnit actor) : base(actor)
        {
        }

        protected override void InfluenceAction()
        {
            if (CombatContext is null)
            {
                throw new InvalidOperationException();
            }

            CombatContext.Combat.Pass();
            base.InfluenceAction();
        }
    }
}