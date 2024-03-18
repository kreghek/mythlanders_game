using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombatDicesTeam.Engine.Ui;

public sealed class Text : UiElementContentBase
{
    private readonly Func<Color, Color> _colorDelegate;
    private readonly SpriteFont _font;
    private readonly Func<string> _textDelegate;
    private readonly Point _textureOffset;

    public Text(Texture2D texture, Point textureOffset, SpriteFont font, Func<Color, Color> colorDelegate,
        Func<string> textDelegate) : base(texture)
    {
        _textureOffset = textureOffset;
        _font = font;
        _colorDelegate = colorDelegate;
        _textDelegate = textDelegate;
    }

    public override Point Size => (_font.MeasureString(_textDelegate()) + new Vector2(CONTENT_MARGIN)).ToPoint();

    protected override Point CalcTextureOffset()
    {
        return _textureOffset;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
    {
        // No background
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.DrawString(_font, _textDelegate(), contentRect.Location.ToVector2(), _colorDelegate(contentColor));
    }
}