using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    internal abstract class DisapearingTextComponent : EwarDrawableComponentBase
    {
        private readonly Vector2 _speed;
        private int _lifetime;
        private Vector2 _position;

        public DisapearingTextComponent(EwarGame game, Vector2 startPosition) : base(game)
        {
            _position = startPosition;
            _lifetime = 2000;
            _speed = new(0, 10f / 1000f);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var ui = Game.Services.GetService<IUiContentStorage>();

            spriteBatch.DrawString(ui.GetMainFont(), GetText(), _position, GetColor());

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

        protected abstract Color GetColor();

        protected abstract string GetText();
    }
}