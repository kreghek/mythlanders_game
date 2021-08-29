using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
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
}
