using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class HitState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly UnitGraphics _graphics;
        private readonly SoundEffectInstance _hitSound;
        private readonly int _index;
        private readonly Action _interaction;
        private double _counter;

        private bool _interactionExecuted;

        public HitState(UnitGraphics graphics, Action attackInteraction,
            SoundEffectInstance hitSound, int index)
            : this(graphics, attackInteraction, default, hitSound, index)
        {
        }

        public HitState(
            UnitGraphics graphics,
            Action attackInteraction,
            AnimationBlocker? animationBlocker,
            SoundEffectInstance hitSound,
            int index)
        {
            _interaction = attackInteraction;
            _hitSound = hitSound;
            _index = index;
            _animationBlocker = animationBlocker;
            _graphics = graphics;
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
                _graphics.PlayAnimation($"Skill{_index}");
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > DURATION)
            {
                IsComplete = true;

                if (_animationBlocker is not null)
                {
                    _animationBlocker.Release();
                }
            }
            else if (_counter > DURATION / 2)
            {
                if (!_interactionExecuted)
                {
                    _hitSound.Play();

                    _interactionExecuted = true;

                    _interaction?.Invoke();
                }
            }
        }
    }
}