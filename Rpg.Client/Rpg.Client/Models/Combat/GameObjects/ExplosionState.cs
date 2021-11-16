using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class ExplosionState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly IList<IInteractionDelivery> _bulletList;
        private readonly SoundEffectInstance _explosionSoundEffect;
        private readonly UnitGraphics _graphics;
        private readonly SoundEffectInstance? _hitSound;
        private readonly int _index;
        private readonly IInteractionDelivery _interactionDelivery;
        private double _counter;

        private bool _interactionExecuted;

        public ExplosionState(UnitGraphics graphics, IInteractionDelivery? interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList)
        {
            _graphics = graphics;
            _interactionDelivery = interactionDelivery;
            _bulletList = interactionDeliveryList;
        }

        public ExplosionState(UnitGraphics graphics, IInteractionDelivery? bulletGameObject,
            IList<IInteractionDelivery> interactionDeliveryList, AnimationBlocker animationBlocker,
            SoundEffectInstance? hitSound,
            int index, SoundEffectInstance explosionSoundEffect) :
            this(graphics, bulletGameObject, interactionDeliveryList)
        {
            _animationBlocker = animationBlocker;
            _hitSound = hitSound;
            _index = index;
            _explosionSoundEffect = explosionSoundEffect;
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
                _explosionSoundEffect.Play();
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
                    if (_interactionDelivery != null)
                    {
                        _bulletList.Add(_interactionDelivery);
                    }

                    _interactionExecuted = true;

                    if (_hitSound is not null)
                    {
                        _hitSound.Play();
                    }
                }
            }
        }
    }
}