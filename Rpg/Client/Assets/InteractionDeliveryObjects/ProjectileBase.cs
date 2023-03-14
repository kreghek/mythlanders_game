using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.InteractionDeliveryObjects
{
    internal abstract class ProjectileBase : IInteractionDelivery
    {
        private readonly AnimationBlocker? _blocker;
        private readonly Vector2 _endPosition;

        private readonly IAnimationFrameSet _frameSet;
        private readonly Sprite _graphics;
        private readonly double _lifetimeDuration;
        private readonly Vector2 _startPosition;

        private double _lifetimeCounter;

        protected ProjectileBase(Vector2 startPosition,
            Vector2 endPosition,
            Texture2D texture,
            IAnimationFrameSet frameSet,
            double lifetimeDuration,
            AnimationBlocker? blocker)
        {
            _graphics = new Sprite(texture)
            {
                Position = startPosition,
                SourceRectangle = new Rectangle(0, 0, 1, 1)
            };

            _startPosition = startPosition;
            _endPosition = endPosition;
            _blocker = blocker;
            _lifetimeDuration = lifetimeDuration;

            _frameSet = frameSet;
        }

        protected Vector2 CurrentPosition
        {
            get
            {
                var t = _lifetimeCounter / _lifetimeDuration;
                var mainPosition = Vector2.Lerp(_startPosition, _endPosition, (float)t);
                var modifierPosition = CalcPositionModifier(t);
                return mainPosition + modifierPosition;
            }
        }

        protected virtual Vector2 CalcPositionModifier(double t)
        {
            return Vector2.Zero;
        }

        protected virtual void DrawForegroundAdditionalEffects(SpriteBatch spriteBatch) { }

        protected virtual void UpdateAdditionalEffects(GameTime gameTime) { }

        public event EventHandler? InteractionPerformed;

        public bool IsDestroyed { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
            {
                return;
            }

            _graphics.Draw(spriteBatch);

            DrawForegroundAdditionalEffects(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (IsDestroyed)
            {
                return;
            }

            _frameSet.Update(gameTime);
            _graphics.SourceRectangle = _frameSet.GetFrameRect();

            if (_lifetimeCounter < _lifetimeDuration)
            {
                _lifetimeCounter += gameTime.ElapsedGameTime.TotalSeconds;

                _graphics.Position = CurrentPosition;
                _graphics.Rotation = MathF.Atan2(_endPosition.Y - _startPosition.Y, _endPosition.X - _startPosition.X);
            }
            else
            {
                if (!IsDestroyed)
                {
                    IsDestroyed = true;
                    _blocker?.Release();
                    InteractionPerformed?.Invoke(this, EventArgs.Empty);
                }
            }

            UpdateAdditionalEffects(gameTime);
        }
    }
}