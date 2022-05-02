﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific.Primitives
{
    internal sealed class SvarogSymbolBurningState : IUnitStateEngine
    {
        private const double STATE_DURATION_SECONDS = 3f;
        private const double SHAKEING_DURATION_SECONDS = STATE_DURATION_SECONDS;

        private readonly SoundEffectInstance _risingPowerSoundEffect;
        private readonly UnitGraphics _graphics;
        private readonly ScreenShaker _screenShaker;
        private bool _isStarted;

        public SvarogSymbolBurningState(UnitGraphics graphics, SvarogSymbolObject svarogSymbol, ScreenShaker screenShaker, SoundEffectInstance risingPowerSoundEffect)
        {
            _graphics = graphics;
            _screenShaker = screenShaker;
            _risingPowerSoundEffect = risingPowerSoundEffect;

            svarogSymbol.RisingPowerCompleted += (_, _) => {
                IsComplete = true;
                // 2 stage is exposion!
                svarogSymbol.SwitchStage(2);
            };
        }

        public bool CanBeReplaced { get; }
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            // Nothing to cancel
        }

        public void Update(GameTime gameTime)
        {
            if (!_isStarted)
            {
                _graphics.PlayAnimation(AnimationSid.Ult);
                _isStarted = true;
                _screenShaker.Start(SHAKEING_DURATION_SECONDS, ShakeDirection.FadeOut);
                _risingPowerSoundEffect.Play();
            }
        }
    }
}