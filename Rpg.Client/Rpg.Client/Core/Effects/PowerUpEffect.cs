using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.Effects
{
    internal sealed class PowerUpEffect : PeriodicEffectBase
    {
        private readonly PowerUpModifier _modifier;

        public PowerUpEffect()
        {
            _modifier = new PowerUpModifier();
        }

        protected override void AfterDispel()
        {
            base.AfterDispel();

            Target.Unit.RemoveModifier(_modifier);
        }

        protected override void AfterImpose()
        {
            base.AfterImpose();

            Target.Unit.AddModifier(_modifier);
        }
    }
}