namespace Rpg.Client.Core.SkillEffects
{
    internal class StunEffect : PeriodicEffectBase
    {
        protected override void InfluenceAction()
        {
            CombatContext.Combat.Pass();
            base.InfluenceAction();
        }

        public StunEffect(ICombatUnit actor, int startDuration) : base(actor, startDuration)
        {
        }

        public StunEffect(ICombatUnit actor) : base(actor)
        {
        }
    }
}