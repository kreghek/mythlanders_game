namespace Rpg.Client.Core.Effects
{
    internal abstract class InstantenousEffectBase : EffectBase
    {
        protected override void AfterImpose()
        {
            Influence();
            Dispel();
            base.AfterImpose();
        }
    }
}