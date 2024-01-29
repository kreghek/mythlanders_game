using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombatDicesTeam.Engine.Ui;

public sealed class Text : ControlBase
{
    private readonly Point _textureOffset;
    private readonly SpriteFont _font;
    private readonly Func<Color, Color> _colorDelegate;
    private readonly Func<string> _textDelegate;

    public Text(Texture2D texture, Point textureOffset, SpriteFont font, Func<Color, Color> colorDelegate, Func<string> textDelegate) : base(texture)
    {
        _textureOffset = textureOffset;
        _font = font;
        _colorDelegate = colorDelegate;
        _textDelegate = textDelegate;
    }

    protected override Point CalcTextureOffset() => _textureOffset;

    protected override Color CalculateColor() => Color.White;

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.DrawString(_font, _textDelegate(), contentRect.Location.ToVector2(), _colorDelegate(contentColor));
    }

    public override Point Size => (_font.MeasureString(_textDelegate()) + new Vector2(CONTENT_MARGIN)).ToPoint();
}