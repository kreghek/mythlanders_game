using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.VoiceCombat;

internal class VoiceCombatOptionButton : ButtonBase
{
    private const int MARGIN = 5;
    private readonly SpriteFont _font;
    private readonly string _optionText;

    public VoiceCombatOptionButton(int number, string resourceSid, string description)
    {
        var localizedText = StringHelper.LineBreaking(GameObjectHelper.GetLocalizedVoiceCombatMove(resourceSid), 30);
        _optionText = $"{number}. {localizedText}\n({description})";

        _font = UiThemeManager.UiContentStorage.GetTitlesFont();
        Number = number;
    }

    /// <summary>
    /// Option number in the option list.
    /// </summary>
    public int Number { get; }

    /// <summary>
    /// Get size of text.
    /// Used in outer code to make the control rect.
    /// </summary>
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

        return Color.SaddleBrown;
    }
}