using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class TextHint : HintBase
    {
        private readonly SpriteFont _font;
        private readonly string _text;

        public TextHint(Texture2D texture, SpriteFont font, string text) : base(texture)
        {
            _font = font;
            _text = text;
        }

        public string Text => _text;

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            spriteBatch.DrawString(_font, Text,
                contentRect.Location.ToVector2() + new Vector2(5, 15),
                Color.Wheat);
        }
    }
}