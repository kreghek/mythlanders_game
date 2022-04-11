namespace Rpg.Client.Core.SkillEffects
{
    internal class StunEffect : PeriodicEffectBase
    {
        public override void MergeWithBase(EffectBase testedEffect)
        {
            throw new System.NotImplementedException();
        }

        protected override void InfluenceAction()
        {
            Combat.Pass();
            base.InfluenceAction();
        }
    }
}