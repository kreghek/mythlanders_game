using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class TextHint : HintBase
    {
        private readonly SpriteFont _spriteFont;
        private readonly string _text;

        public TextHint(Texture2D texture, SpriteFont spriteFont, string text) : base(texture)
        {
            _spriteFont = spriteFont;
            _text = text;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            spriteBatch.DrawString(_spriteFont, _text,
                contentRect.Location.ToVector2() + new Vector2(5, 15),
                Color.Wheat);
        }
    }
}
