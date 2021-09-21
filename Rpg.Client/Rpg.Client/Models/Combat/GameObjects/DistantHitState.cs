using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class DistantHitState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly Action _attackInteraction;
        private readonly BulletGameObject _bulletGameObject;
        private readonly IList<BulletGameObject> _bulletList;
        private readonly UnitGraphics _graphics;
        private readonly SoundEffectInstance? _hitSound;

        private double _counter;

        private bool _interactionExecuted;

        public DistantHitState(UnitGraphics graphics, Action attackInteraction,
            BulletGameObject? bulletGameObject, IList<BulletGameObject> bulletList)
        {
            _graphics = graphics;
            _attackInteraction = attackInteraction;
            _bulletGameObject = bulletGameObject;
            _bulletList = bulletList;
        }

        public DistantHitState(UnitGraphics graphics, Action attackInteraction,
            BulletGameObject? bulletGameObject, IList<BulletGameObject> bulletList, AnimationBlocker animationBlocker,
            Microsoft.Xna.Framework.Audio.SoundEffectInstance? hitSound) :
            this(graphics, attackInteraction, bulletGameObject, bulletList)
        {
            _animationBlocker = animationBlocker;
            _hitSound = hitSound;
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