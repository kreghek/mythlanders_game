using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class UnitAttackState : IUnitStateEngine
    {
        private IUnitStateEngine[] _subStates;

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public UnitAttackState(SpriteContainer graphicsRoot, SpriteContainer targetGraphicsRoot, AnimationBlocker blocker)
        {
            _subStates = new IUnitStateEngine[]
                {
                    new MoveToTarget(graphicsRoot, targetGraphicsRoot),
                    new MoveBack(graphicsRoot, targetGraphicsRoot, blocker)
                };
            _blocker = blocker;
        }

        public void Cancel()
        {
            if (IsComplete)
            {
                return;
            }

            _blocker.Release();
        }

        public void Update(GameTime gameTime)
        {
            if (_subStateIndex < _subStates.Length)
            {
                var currentSubState = _subStates[_subStateIndex];
                if (currentSubState.IsComplete)
                {
                    _subStateIndex++;
                }
                else
                {
                    currentSubState.Update(gameTime);
                }
            }
            else
            {
                IsComplete = true;
            }
        }

        private int _subStateIndex = 0;
        private readonly AnimationBlocker _blocker;
    }

    internal class MoveToTarget : IUnitStateEngine
    {
        private readonly Vector2 _startPosition;
        private readonly Vector2 _targetPosition;
        private readonly SpriteContainer _graphicsRoot;
        private double _counter = 0;

        public MoveToTarget(SpriteContainer graphicsRoot, SpriteContainer targetGraphicsRoot)
        {
            _startPosition = graphicsRoot.Position;
            _targetPosition = targetGraphicsRoot.Position;
            _graphicsRoot = graphicsRoot;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (IsComplete)
            {
                return;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            if (_counter <= 1)
            {
                _counter += gameTime.ElapsedGameTime.TotalSeconds;

                _graphicsRoot.Position = Vector2.Lerp(_startPosition, _targetPosition, (float)_counter);
            }
            else
            {
                
                IsComplete = true;
                _graphicsRoot.Position = _targetPosition;
            }
        }
    }

    internal class MoveBack : IUnitStateEngine
    {
        private readonly Vector2 _startPosition;
        private readonly Vector2 _targetPosition;
        private readonly SpriteContainer _graphicsRoot;
        private readonly AnimationBlocker _blocker;
        private double _counter = 0;

        public MoveBack(SpriteContainer graphicsRoot, SpriteContainer targetGraphicsRoot, AnimationBlocker blocker)
        {
            _startPosition = graphicsRoot.Position;
            _targetPosition = targetGraphicsRoot.Position;
            _graphicsRoot = graphicsRoot;
            _blocker = blocker;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (IsComplete)
            {
                return;
            }

            _blocker.Release();
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            if (_counter <= 1)
            {
                _counter += gameTime.ElapsedGameTime.TotalSeconds;

                _graphicsRoot.Position = Vector2.Lerp(_startPosition, _targetPosition, (float)(1-_counter));
            }
            else
            {
                IsComplete = true;

                _blocker.Release();

                _graphicsRoot.Position = _startPosition;
            }
        }
    }
}
