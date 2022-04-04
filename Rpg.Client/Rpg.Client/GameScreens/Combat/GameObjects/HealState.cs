using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class HealState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly UnitGraphics _graphics;
        private readonly Action _healInteraction;
        private readonly SoundEffectInstance _hitSound;
        private readonly AnimationSid _animationSid;
        private double _counter;

        private bool _interactionExecuted;

        public HealState(UnitGraphics graphics, Action healInteraction, SoundEffectInstance hitSound, AnimationSid animationSid)
        {
            _graphics = graphics;
            _healInteraction = healInteraction;
            _hitSound = hitSound;
            _animationSid = animationSid;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _graphics.PlayAnimation(_animationSid);
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > DURATION)
            {
                IsComplete = true;
            }
            else if (_counter > DURATION / 2)
            {
                if (!_interactionExecuted)
                {
                    _healInteraction?.Invoke();

                    _interactionExecuted = true;
                }

                _hitSound.Play();
            }
        }
    }
}