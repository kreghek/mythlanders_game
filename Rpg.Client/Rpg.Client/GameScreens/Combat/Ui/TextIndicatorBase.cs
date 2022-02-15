﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal abstract class TextIndicatorBase : EwarRenderableBase

    {
        private readonly SpriteFont _font;
        private readonly Vector2 _speed;
        private int _lifetime;
        private Vector2 _position;

        public TextIndicatorBase(Vector2 startPosition, SpriteFont font)
        {
            _position = startPosition;
            _font = font;
            _lifetime = 2000;
            _speed = new(0, 10f / 1000f);
        }

        public override void Update(GameTime gameTime)
        {
            if (_lifetime <= 0)
            {
                Parent?.RemoveChild(this);

                return;
            }

            var elapsed = gameTime.ElapsedGameTime.Milliseconds;

            _position += _speed * elapsed;

            _lifetime -= elapsed;

            base.Update(gameTime);
        }

        protected override void DoDraw(SpriteBatch spriteBatch, float zIndex)
        {
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    spriteBatch.DrawString(_font, GetText(), _position + new Vector2(x, y), Color.DarkCyan);
                }
            }

            spriteBatch.DrawString(_font, GetText(), _position, GetColor());
        }

        protected abstract Color GetColor();

        protected abstract string GetText();
    }
}