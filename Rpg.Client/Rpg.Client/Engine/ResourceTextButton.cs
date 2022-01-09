using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal class ResourceTextButton : ButtonBase
    {
        private readonly SpriteFont _font;
        private readonly string _resourceSid;

        public ResourceTextButton(string resourceSid, Texture2D texture, SpriteFont font) : this(resourceSid, texture, font, Rectangle.Empty) { }

        public ResourceTextButton(string resourceSid, Texture2D texture, SpriteFont font, Rectangle rect) : base(
            texture, rect)
        {
            _resourceSid = resourceSid;

            _font = font;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            var localizedTitle = UiResource.ResourceManager.GetString(_resourceSid) ?? _resourceSid;

            var textSize = _font.MeasureString(localizedTitle);
            var widthDiff = contentRect.Width - textSize.X;
            var heightDiff = contentRect.Height - textSize.Y;
            var textPosition = new Vector2(
                (widthDiff / 2) + contentRect.Left,
                (heightDiff / 2) + contentRect.Top);

            spriteBatch.DrawString(_font, localizedTitle, textPosition, Color.SaddleBrown);
        }
    }
}