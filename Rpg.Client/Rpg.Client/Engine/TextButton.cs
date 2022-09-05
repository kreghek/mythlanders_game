using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal class TextButton : ButtonBase
    {
        private readonly SpriteFont _font;

        private readonly string _title;

        public TextButton(string title)
        {
            _title = title;

            _font = UiThemeManager.UiContentStorage.GetMainFont();
        }

        protected override Point CalcTextureOffset()
        {
            return Point.Zero;
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