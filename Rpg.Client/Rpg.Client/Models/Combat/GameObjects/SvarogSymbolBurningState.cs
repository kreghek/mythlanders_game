using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class SvarogSymbolBurningState : IUnitStateEngine
    {
        private const double DURATION = 3f;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly IList<IInteractionDelivery> _bulletList;
        private readonly UnitGraphics _graphics;
        private readonly SoundEffectInstance? _hitSound;
        private readonly int _index;
        private readonly SoundEffectInstance _risingPowerSoundEffect;
        private readonly IInteractionDelivery _interactionDelivery;
        private readonly ScreenShaker _screenShaker;
        private double _counter;

        private bool _interactionExecuted;

        public SvarogSymbolBurningState(UnitGraphics graphics, IInteractionDelivery? interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList,
            ScreenShaker screenShaker)
        {
            _graphics = graphics;
            _interactionDelivery = interactionDelivery;
            _bulletList = interactionDeliveryList;
            _screenShaker = screenShaker;
        }

        public SvarogSymbolBurningState(UnitGraphics graphics, IInteractionDelivery? bulletGameObject,
            IList<IInteractionDelivery> interactionDeliveryList, AnimationBlocker animationBlocker,
            SoundEffectInstance? hitSound,
            int index,
            ScreenShaker screenShaker, SoundEffectInstance risingPowerSoundEffect) :
            this(graphics, bulletGameObject, interactionDeliveryList, screenShaker)
        {
            _animationBlocker = animationBlocker;
            _hitSound = hitSound;
            _index = index;
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
                _risingPowerSoundEffect.Play();
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > DURATION)
            {
                IsComplete = true;
            }
        }
    }
}