using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class DistantHitState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly BulletGameObject _bulletGameObject;
        private readonly IList<IInteractionDelivery> _bulletList;
        private readonly UnitGraphics _graphics;
        private readonly SoundEffectInstance? _hitSound;
        private readonly int _index;
        private double _counter;

        private bool _interactionExecuted;

        public DistantHitState(UnitGraphics graphics, BulletGameObject? bulletGameObject,
            IList<IInteractionDelivery> interactionDeliveryList)
        {
            _graphics = graphics;
            _bulletGameObject = bulletGameObject;
            _bulletList = interactionDeliveryList;
        }

        public DistantHitState(UnitGraphics graphics, Action attackInteraction,
            BulletGameObject? bulletGameObject, IList<IInteractionDelivery> interactionDeliveryList, AnimationBlocker animationBlocker,
            SoundEffectInstance? hitSound, int index) :
            this(graphics, bulletGameObject, interactionDeliveryList)
        {
            _animationBlocker = animationBlocker;
            _hitSound = hitSound;
            _index = index;
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
                    if (_bulletGameObject != null)
                    {
                        _bulletList.Add(_bulletGameObject);
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