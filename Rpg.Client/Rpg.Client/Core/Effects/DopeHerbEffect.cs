namespace Rpg.Client.Core.Effects
{
    internal class StunEffect : PeriodicEffectBase
    {
        protected override void InfluenceAction()
        {
            Combat.Pass();
            base.InfluenceAction();
        }
    }
}