using System.Reflection;
using System.Resources;

using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Crisis.Ui;

internal class CrisisAftermathButton : ButtonBase
{
    private const int MARGIN = 5;
    private readonly ResourceManager _dialogueResourceManager;
    private readonly SpriteFont _font;
    private readonly string _textSid;

    public CrisisAftermathButton(int number, string textSid)
    {
        _font = UiThemeManager.UiContentStorage.GetTitlesFont();
        Number = number;
        _textSid = textSid;

        var assembly = Assembly.GetExecutingAssembly();
        _dialogueResourceManager = new ResourceManager("Client.DialogueResources", assembly);
    }

    public int Number { get; }

    public Vector2 GetContentSize()
    {
        var optionText = GetOptionText();

        var textSize = _font.MeasureString(optionText) + new Vector2(MARGIN * 2, MARGIN * 2);
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

    protected Color CalculateTextColor()
    {
        if (_buttonState == UiButtonState.Hover || _buttonState == UiButtonState.Pressed)
        {
            return Color.Wheat;
        }

        return Color.SaddleBrown;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
    {
        var textSize = GetContentSize();
        var heightDiff = contentRect.Height - textSize.Y;
        var textPosition = new Vector2(
            contentRect.Left + MARGIN,
            heightDiff / 2 + contentRect.Top);

        var textColor = CalculateTextColor();

        var optionText = GetOptionText();

        spriteBatch.DrawString(_font, optionText, textPosition, textColor);
    }

    private string GetOptionText()
    {
        var text = _dialogueResourceManager.GetString(_textSid) ?? _textSid;
        var multilineText = StringHelper.LineBreaking(text, 40);
        return multilineText;
    }
}