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

        public DisapearingTextComponent(Vector2 startPosition)
        {
            _position = startPosition;
            _lifetime = 2000;
            _speed = new(0, 10f / 1000f);
        }

        protected override void DoDraw(SpriteBatch spriteBatch, float zIndex)
        {
            spriteBatch.Begin();
            var ui = Game.Services.GetService<IUiContentStorage>();

            spriteBatch.DrawString(ui.GetMainFont(), GetText(), _position, GetColor());
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (_lifetime <= 0)
            {
                if (Parent != null)
                    Parent.RemoveChild(this);
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