using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class TextHint : HintBase
    {
        private readonly SpriteFont _font;

        public TextHint(string text)
        {
            _font = UiThemeManager.UiContentStorage.GetMainFont();
            Text = text;
        }

        public string Text { get; }

        protected override Point CalcTextureOffset()
        {
            return Point.Zero;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            spriteBatch.DrawString(_font, Text,
                contentRect.Location.ToVector2() + new Vector2(5, 15),
                Color.Wheat);
        }
    }
}