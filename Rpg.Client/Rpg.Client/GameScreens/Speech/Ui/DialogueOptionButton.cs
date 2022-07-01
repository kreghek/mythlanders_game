using System.Reflection;
using System.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Speech.Ui
{
    internal class DialogueOptionButton : ButtonBase
    {
        private readonly SpriteFont _font;
        private readonly string _optionText;

        public DialogueOptionButton(string resourceSid, Texture2D texture, SpriteFont font) : base(
            texture, Rectangle.Empty)
        {
            _optionText = SpeechVisualizationHelper.PrepareLocalizedText(resourceSid);

            _font = font;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            var textSize = _font.MeasureString(_optionText);
            var widthDiff = contentRect.Width - textSize.X;
            var heightDiff = contentRect.Height - textSize.Y;
            var textPosition = new Vector2(
                (widthDiff / 2) + contentRect.Left,
                (heightDiff / 2) + contentRect.Top);

            spriteBatch.DrawString(_font, _optionText, textPosition, Color.SaddleBrown);
        }

        public Vector2 GetContentSize()
        {
            var textSize = _font.MeasureString(_optionText);
            return textSize;
        }
    }
}