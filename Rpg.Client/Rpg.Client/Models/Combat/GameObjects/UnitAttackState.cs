using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class UnitAttackState : IUnitStateEngine
    {
        private readonly Vector2 _startPosition;
        private readonly Vector2 _targetPosition;
        private readonly SpriteContainer _graphicsRoot;
        private readonly AnimationBlocker _blocker;
        private double _counter = 0;

        public UnitAttackState(SpriteContainer graphicsRoot, SpriteContainer targetGraphicsRoot, AnimationBlocker blocker)
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

                _graphicsRoot.Position = Vector2.Lerp(_startPosition, _targetPosition, (float)_counter);
            }
            else
            {
                IsComplete = true;

                _blocker.Release();
                IsComplete = true;
                _graphicsRoot.Position = _startPosition;
            }
        }
    }
}
