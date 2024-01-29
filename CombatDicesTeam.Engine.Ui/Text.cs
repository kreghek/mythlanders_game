using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine;

public sealed class Text : ControlBase
{
    private readonly Point _textureOffset;
    private readonly SpriteFont _font;
    private readonly Color _color;
    private readonly Func<string> _textDelegate;

    public Text(Texture2D texture, Point textureOffset, SpriteFont font, Color color, Func<string> textDelegate) : base(texture)
    {
        _textureOffset = textureOffset;
        _font = font;
        _color = color;
        _textDelegate = textDelegate;
    }

    protected override Point CalcTextureOffset() => _textureOffset;

    protected override Color CalculateColor() => _color;

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.DrawString(_font, _textDelegate(), contentRect.Location.ToVector2(), contentColor);
    }
}