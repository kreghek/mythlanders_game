using Core.Crises;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.GameScreens.Crisis.Ui;

internal class CrisisAftermathButton : ButtonBase
{
    private const int MARGIN = 5;
    private readonly SpriteFont _font;
    private readonly string _optionText;

    public CrisisAftermathButton(int number, CrisisAftermathSid sid)
    {
        var localizedTitle = GameObjectResources.ResourceManager.GetString(sid.ResourceName) ?? sid.ResourceName;

        _optionText = $"{number}. {localizedTitle}";

        _font = UiThemeManager.UiContentStorage.GetTitlesFont();
        Number = number;
    }

    public int Number { get; }

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
        spriteBatch.DrawString(_font, _optionText, textPosition, textColor);
    }
}