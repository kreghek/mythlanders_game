using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.InteractionDeliveryObjects;
using Rpg.Client.Core;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.States.HeroSpecific.Primitives
{
    internal sealed class SvarogSymbolBurningState : IActorVisualizationState
    {
        private const double STATE_DURATION_SECONDS = 3f;
        private const double SHAKEING_DURATION_SECONDS = STATE_DURATION_SECONDS;
        private readonly UnitGraphics _graphics;

        private readonly SoundEffectInstance _risingPowerSoundEffect;
        private readonly ScreenShaker _screenShaker;
        private bool _isStarted;

        public SvarogSymbolBurningState(UnitGraphics graphics, SvarogSymbolObject svarogSymbol,
            ScreenShaker screenShaker, SoundEffectInstance risingPowerSoundEffect)
        {
            _graphics = graphics;
            _screenShaker = screenShaker;
            _risingPowerSoundEffect = risingPowerSoundEffect;

            svarogSymbol.RisingPowerCompleted += (_, _) =>
            {
                IsComplete = true;
                // 2 stage is exposion!
                svarogSymbol.SwitchStage(2);
            };
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            // Nothing to cancel
        }

        public void Update(GameTime gameTime)
        {
            if (!_isStarted)
            {
                _graphics.PlayAnimation(PredefinedAnimationSid.Ult);
                _isStarted = true;
                _screenShaker.Start(SHAKEING_DURATION_SECONDS, ShakeDirection.FadeOut);
                _risingPowerSoundEffect.Play();
            }
        }
    }
}