using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombatDicesTeam.Engine.Ui;

public sealed class Image : ControlBase
{
    private readonly Texture2D _image;
    private readonly Rectangle _sourceRect;
    private readonly Point _textureOffset;
    private readonly Func<Color>? _colorDelegate;
    private readonly Point? _targetSize;

    public Image(Texture2D image, Rectangle sourceRect, Texture2D texture, Point textureOffset) : this(image, sourceRect, texture, textureOffset, ()=> Color.White, null)
    {
    }

    public Image(Texture2D image, Rectangle sourceRect, Texture2D texture, Point textureOffset, Func<Color> colorDelegate, Point? size) : base(texture)
    {
        _image = image;
        _sourceRect = sourceRect;
        _textureOffset = textureOffset;
        _colorDelegate = colorDelegate;
        _targetSize = size;
    }

    public override Point Size => _targetSize ?? _sourceRect.Size;

    protected override Point CalcTextureOffset()
    {
        return _textureOffset;
    }

    protected override Color CalculateColor()
    {
        return _colorDelegate?.Invoke() ?? Color.White;
    }

    protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
    {
        // Do not draw background of image
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.Draw(_image, new Rectangle(contentRect.Location, Size), _sourceRect, contentColor);
    }
}