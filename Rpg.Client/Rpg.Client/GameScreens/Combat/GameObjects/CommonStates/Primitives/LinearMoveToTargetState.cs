using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects.CommonStates.Primitives
{
    internal class LinearMoveToTargetState : IUnitStateEngine
    {
        private const double DURATION_SECONDS = 0.25;
        private readonly PredefinedAnimationSid? _predefinedAnimationSid;
        private readonly UnitGraphics _graphics;
        private readonly SpriteContainer _graphicsRoot;

        private readonly Vector2 _startPosition;
        private readonly Vector2 _targetPosition;
        private readonly IAnimationFrameSet? _animation;
        private double _counter;

        public LinearMoveToTargetState(UnitGraphics graphics, SpriteContainer graphicsRoot, Vector2 targetPosition,
            PredefinedAnimationSid animationSid)
        {
            _startPosition = graphicsRoot.Position;
            _targetPosition = targetPosition;
            _predefinedAnimationSid = animationSid;
            _graphics = graphics;
            _graphicsRoot = graphicsRoot;
        }

        public LinearMoveToTargetState(UnitGraphics graphics, SpriteContainer graphicsRoot, Vector2 targetPosition,
            IAnimationFrameSet animation)
        {
            _startPosition = graphicsRoot.Position;
            _targetPosition = targetPosition;
            _animation = animation;
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
                if (_predefinedAnimationSid is not null)
                {
                    _graphics.PlayAnimation(_predefinedAnimationSid.Value);
                }
                else if (_animation is not null)
                {
                    _graphics.PlayAnimation(_animation);
                }
                else
                {
                    _graphics.PlayAnimation(PredefinedAnimationSid.Idle);
                    Debug.Fail("Any animation must be defined in the constructor.");
                }
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