namespace Rpg.Client.Core.SkillEffects
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