using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class HealState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly UnitGraphics _graphics;
        private readonly Action _healInteraction;
        private readonly SoundEffectInstance _hitSound;
        private double _counter;

        private bool _interactionExecuted;

        public HealState(UnitGraphics graphics, Action healInteraction, SoundEffectInstance hitSound)
        {
            _graphics = graphics;
            _healInteraction = healInteraction;
            _hitSound = hitSound;
        }

        public bool CanBeReplaced { get; }
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _graphics.PlayAnimation("Hit");
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