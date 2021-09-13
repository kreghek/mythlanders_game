using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class HitState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly IUnitInteraction _attackInteraction;
        private readonly SoundEffectInstance _hitSound;
        private readonly UnitGraphics _graphics;

        private double _counter;

        private bool _interactionExecuted;

        public HitState(UnitGraphics graphics, IUnitInteraction attackInteraction, Microsoft.Xna.Framework.Audio.SoundEffectInstance hitSound)
        {
            _graphics = graphics;
            _attackInteraction = attackInteraction;
            _hitSound = hitSound;
        }

        public HitState(
            UnitGraphics graphics,
            IUnitInteraction attackInteraction,
            AnimationBlocker animationBlocker,
            Microsoft.Xna.Framework.Audio.SoundEffectInstance hitSound) :
            this(graphics, attackInteraction, hitSound)
        {
            _animationBlocker = animationBlocker;
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
                _graphics.PlayAnimation("Hit");
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
                    _attackInteraction.Execute();

                    _hitSound.Play();

                    _interactionExecuted = true;
                }
            }
        }
    }
}