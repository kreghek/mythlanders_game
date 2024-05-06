using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombatDicesTeam.Engine.Ui;

public sealed class Image : ControlBase
{
    private readonly Texture2D _image;
    private readonly Rectangle _sourceRect;
    private readonly Point _textureOffset;

    public Image(Texture2D image, Rectangle sourceRect, Texture2D texture, Point textureOffset) : base(texture)
    {
        _image = image;
        _sourceRect = sourceRect;
        _textureOffset = textureOffset;
    }

    public override Point Size => _sourceRect.Size;

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
        // Do not draw background of image
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.Draw(_image, contentRect.Location.ToVector2(), _sourceRect, contentColor);
    }
}