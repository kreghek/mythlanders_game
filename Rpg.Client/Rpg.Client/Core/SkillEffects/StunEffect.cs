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
            CombatContext.Combat.Pass();
            base.InfluenceAction();
        }
    }
}