using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

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
            BulletGameObject? bulletGameObject, IList<BulletGameObject> bulletList, AnimationBlocker animationBlocker) :
            this(graphics, attackInteraction, bulletGameObject, bulletList)
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
                //if (!IsComplete)
                //    _attackInteraction?.Invoke();

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
                        _bulletList.Add(_bulletGameObject);

                    _interactionExecuted = true;
                }
            }
        }
    }
}