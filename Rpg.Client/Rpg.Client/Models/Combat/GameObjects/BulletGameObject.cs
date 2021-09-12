using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class BulletGameObject
    {
        private const double DURATION_SECONDS = 1.0;
        private readonly AttackInteraction _attackInteraction;
        private readonly AnimationBlocker _blocker;
        private readonly Vector2 _endPosition;
        private readonly Sprite _graphics;
        private readonly Vector2 _startPosition;
        private double _counter;

        public BulletGameObject(Vector2 startPosition, Vector2 endPosition, GameObjectContentStorage contentStorage,
            AnimationBlocker blocker, AttackInteraction attackInteraction)
        {
            _graphics = new Sprite(contentStorage.GetBulletGraphics());
            _startPosition = startPosition;
            _endPosition = endPosition;
            _blocker = blocker;
            _attackInteraction = attackInteraction;
        }

        public bool IsDestroyed { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
            {
                return;
            }

            _graphics.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (IsDestroyed)
            {
                return;
            }

            if (_counter < DURATION_SECONDS)
            {
                _counter += gameTime.ElapsedGameTime.TotalSeconds;
                var t = _counter / DURATION_SECONDS;
                _graphics.Position = Vector2.Lerp(_startPosition, _endPosition, (float)t);
            }
            else
            {
                if (!IsDestroyed)
                {
                    IsDestroyed = true;
                    _blocker.Release();
                    _attackInteraction.Execute();
                }
            }
        }
    }
}