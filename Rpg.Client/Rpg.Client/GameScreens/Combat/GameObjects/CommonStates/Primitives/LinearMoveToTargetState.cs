﻿using System;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects.CommonStates.Primitives
{
    internal class LinearMoveToTargetState : IUnitStateEngine
    {
        private const double DURATION_SECONDS = 0.25;
        private readonly PredefinedAnimationSid _animationSid;
        private readonly UnitGraphics _graphics;
        private readonly SpriteContainer _graphicsRoot;

        private readonly Vector2 _startPosition;
        private readonly Vector2 _targetPosition;
        private double _counter;

        public LinearMoveToTargetState(UnitGraphics graphics, SpriteContainer graphicsRoot, Vector2 targetPosition,
            PredefinedAnimationSid animationSid)
        {
            _startPosition = graphicsRoot.Position;
            _targetPosition = targetPosition;
            _animationSid = animationSid;
            _graphics = graphics;
            _graphicsRoot = graphicsRoot;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (IsComplete)
            {
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            if (_counter == 0)
            {
                _graphics.PlayAnimation(_animationSid);
            }

            if (_counter <= DURATION_SECONDS)
            {
                _counter += gameTime.ElapsedGameTime.TotalSeconds;

                var t = _counter / DURATION_SECONDS;

                var horizontalPosition = Vector2.Lerp(_startPosition, _targetPosition, (float)t);

                //var jumpTopPosition = Vector2.UnitY * -24 * (float)Math.Sin((float)_counter / DURATION * Math.PI);

                //var fullPosition = horizontalPosition + jumpTopPosition;

                _graphicsRoot.Position = horizontalPosition;
            }
            else
            {
                IsComplete = true;
                _graphicsRoot.Position = _targetPosition;
            }
        }

        public event EventHandler? Completed;
    }
}