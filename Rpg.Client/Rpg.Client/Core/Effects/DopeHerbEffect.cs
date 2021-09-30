namespace Rpg.Client.Core.Effects
{
    internal class DopeHerbEffect : PeriodicEffectBase
    {
        protected override void InfluenceAction()
        {
            Combat.Pass();
            base.InfluenceAction();
        }
    }
}