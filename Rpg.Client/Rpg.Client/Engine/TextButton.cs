using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal class TextButton : ButtonBase
    {
        private readonly SpriteFont _font;

        private readonly string _title;

        public TextButton(string title, Texture2D texture, SpriteFont font) : this(title, texture, font,
            Rectangle.Empty)
        {
        }

        public TextButton(string title, Texture2D texture, SpriteFont font, Rectangle rect) : base(texture, rect)
        {
            _title = title;

            _font = font;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            var textSize = _font.MeasureString(_title);
            var widthDiff = contentRect.Width - textSize.X;
            var heightDiff = contentRect.Height - textSize.Y;
            var textPosition = new Vector2(
                (widthDiff / 2) + contentRect.Left,
                (heightDiff / 2) + contentRect.Top);

            spriteBatch.DrawString(_font, _title, textPosition, Color.SaddleBrown);
        }
    }
}