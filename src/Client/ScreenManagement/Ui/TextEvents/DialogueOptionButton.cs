using Client.Engine;
using Client.GameScreens.TextDialogue.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.ScreenManagement.Ui.TextEvents;

internal class DialogueOptionButton : ButtonBase
{
    private const int MARGIN = 5;
    private readonly SpriteFont _font;
    private readonly string _optionText;

    public DialogueOptionButton(int number, string resourceSid)
    {
        _optionText = $"{number}. {SpeechVisualizationHelper.PrepareLocalizedText(resourceSid).text}";

        _font = UiThemeManager.UiContentStorage.GetTitlesFont();
        Number = number;
        ResourceSid = resourceSid;
    }

    public int Number { get; }
    public string ResourceSid { get; }

    public Vector2 GetContentSize()
    {
        var textSize = _font.MeasureString(_optionText) + new Vector2(MARGIN * 2, MARGIN * 2);
        return textSize;
    }

    protected override Point CalcTextureOffset()
    {
        if (_buttonState == UiButtonState.Hover || _buttonState == UiButtonState.Pressed)
        {
            return ControlTextures.OptionHover;
        }

        return ControlTextures.OptionNormal;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
    {
        var textSize = GetContentSize();
        var heightDiff = contentRect.Height - textSize.Y;
        var textPosition = new Vector2(
            contentRect.Left + MARGIN,
            heightDiff / 2 + contentRect.Top);

        var textColor = CalculateTextColor();
        spriteBatch.DrawString(_font, _optionText, textPosition, textColor);
    }

    private Color CalculateTextColor()
    {
        if (_buttonState == UiButtonState.Hover || _buttonState == UiButtonState.Pressed)
        {
            return Color.Wheat;
        }

        if (!IsEnabled)
        {
            return Color.Gray;
        }

        return Color.SaddleBrown;
    }
}