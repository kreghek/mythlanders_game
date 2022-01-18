using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal class IndicatorTextButton: ButtonBase
    {
        private readonly string _resourceSid;
        private readonly SpriteFont _font;

        public IndicatorTextButton(string resourceSid, Texture2D texture, SpriteFont font) : base(texture, Rectangle.Empty)
        {
            _resourceSid = resourceSid;
            _font = font;
        }

        private float _counter;

        protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
        {
            _counter += 0.1f;
            if (_counter > 1)
            {
                _counter = 0;
            }

            var totalColor = Color.Lerp(color, Color.Red, _counter);

            base.DrawBackground(spriteBatch, totalColor);
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            var localizedTitle = UiResource.ResourceManager.GetString(_resourceSid) ?? _resourceSid;

            var (textWidth, textHeight) = _font.MeasureString(localizedTitle);
            var widthDiff = contentRect.Width - textWidth;
            var heightDiff = contentRect.Height - textHeight;
            var textPosition = new Vector2(
                (widthDiff / 2) + contentRect.Left,
                (heightDiff / 2) + contentRect.Top);

            spriteBatch.DrawString(_font, localizedTitle, textPosition, Color.SaddleBrown);
        }
    }
}