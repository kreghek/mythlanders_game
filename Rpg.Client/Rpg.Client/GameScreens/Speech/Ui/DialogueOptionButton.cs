using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Speech.Ui
{
    internal class DialogueOptionButton : ButtonBase
    {
        private readonly SpriteFont _font;
        private readonly string _resourceSid;

        public DialogueOptionButton(string resourceSid, Texture2D texture, SpriteFont font) : base(
            texture, Rectangle.Empty)
        {
            _resourceSid = resourceSid;

            _font = font;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            var optionLocalizedText = GetOptionLocalizedText(_resourceSid);

            var textSize = _font.MeasureString(optionLocalizedText);
            var widthDiff = contentRect.Width - textSize.X;
            var heightDiff = contentRect.Height - textSize.Y;
            var textPosition = new Vector2(
                (widthDiff / 2) + contentRect.Left,
                (heightDiff / 2) + contentRect.Top);

            spriteBatch.DrawString(_font, optionLocalizedText, textPosition, Color.SaddleBrown);
        }

        private static string GetOptionLocalizedText(string resourceSid)
        {
            var rm = DialogueResources.ResourceManager;

            return rm.GetString(resourceSid) ?? $"#{resourceSid}";
        }
    }
}