using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class SvarogSymbolBurningState : IUnitStateEngine
    {
        private const double DURATION = 3f;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly SoundEffectInstance? _risingPowerSoundEffect;
        private readonly ScreenShaker _screenShaker;
        private double _counter;

        public SvarogSymbolBurningState(ScreenShaker screenShaker)
        {
            _screenShaker = screenShaker;
        }

        public SvarogSymbolBurningState(AnimationBlocker animationBlocker,
            ScreenShaker screenShaker, SoundEffectInstance risingPowerSoundEffect) :
            this(screenShaker)
        {
            _animationBlocker = animationBlocker;
            _risingPowerSoundEffect = risingPowerSoundEffect;
        }

        public bool CanBeReplaced { get; }
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (_animationBlocker is not null)
            {
                _animationBlocker.Release();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _screenShaker.Start(DURATION, ShakeDirection.FadeOut);
                _risingPowerSoundEffect?.Play();
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > DURATION)
            {
                IsComplete = true;
            }
        }
    }
}