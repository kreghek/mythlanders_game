using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine;

internal sealed class TextHint : HintBase
{
    private readonly SpriteFont _font;

    public TextHint(string text)
    {
        _font = UiThemeManager.UiContentStorage.GetMainFont();
        Text = text;
    }

    public override Point Size => (_font.MeasureString(Text) + new Vector2(CONTENT_MARGIN * 2)).ToPoint();

    public string Text { get; }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.DrawString(_font, Text,
            contentRect.Location.ToVector2(),
            Color.Wheat);
    }
}