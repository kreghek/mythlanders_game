using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Engine;
using Rpg.Client.GameComponents;

namespace Rpg.Client.Models.Combat.Ui
{
    internal class HpChanged : EwarDrawableComponentBase
    {
        private const int LIFETIME = 2000;

        private readonly int _amount;
        private readonly Color _color;

        private int _lifetime;
        private Vector2 _position;

        private readonly Vector2 _speed = new(0, 10f / 1000f);

        public HpChanged(EwarGame game, int amount, Vector2 startPosition) : base(game)
        {
            _amount = amount;
            _color = _amount >= 0 ? Color.LightGreen : Color.Red;
            _position = startPosition;
            _lifetime = LIFETIME;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var ui = Game.Services.GetService<IUiContentStorage>();

            spriteBatch.DrawString(ui.GetMainFont(), $"{_amount}", _position, _color);

            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (_lifetime <= 0)
            {
                Remove();
                return;
            }

            var elapsed = gameTime.ElapsedGameTime.Milliseconds;

            _position += _speed * elapsed;

            _lifetime -= elapsed;

            base.Update(gameTime);
        }
    }
}