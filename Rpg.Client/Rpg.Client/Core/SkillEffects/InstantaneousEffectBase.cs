namespace Rpg.Client.Core.SkillEffects
{
    internal abstract class InstantaneousEffectBase : EffectBase
    {
        protected override void AfterImpose()
        {
            Influence();
            Dispel();
            base.AfterImpose();
        }
    }
}