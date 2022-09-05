using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Speech.Ui
{
    internal class DialogueOptionButton : ButtonBase
    {
        private const int MARGIN = 5;
        private readonly SpriteFont _font;
        private readonly string _optionText;

        public DialogueOptionButton(string resourceSid)
        {
            _optionText = SpeechVisualizationHelper.PrepareLocalizedText(resourceSid);

            _font = UiThemeManager.UiContentStorage.GetTitlesFont();
        }

        protected override Point CalcTextureOffset()
        {
            if (_buttonState == UiButtonState.Hover || _buttonState == UiButtonState.Pressed)
            {
                return ControlTextures.OptionHover;
            }
            else
            {
                return ControlTextures.OptionNormal;
            }
        }

        protected Color CalculateTextColor()
        {
            if (_buttonState == UiButtonState.Hover || _buttonState == UiButtonState.Pressed)
            {
                return Color.Wheat;
            }
            else
            {
                return Color.SaddleBrown;
            }
        }

        public Vector2 GetContentSize()
        {
            var textSize = _font.MeasureString(_optionText) + new Vector2(MARGIN * 2, MARGIN * 2);
            return textSize;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            var textSize = GetContentSize();
            var heightDiff = contentRect.Height - textSize.Y;
            var textPosition = new Vector2(
                contentRect.Left + MARGIN,
                (heightDiff / 2) + contentRect.Top);

            var textColor = CalculateTextColor();
            spriteBatch.DrawString(_font, _optionText, textPosition, textColor);
        }
    }
}