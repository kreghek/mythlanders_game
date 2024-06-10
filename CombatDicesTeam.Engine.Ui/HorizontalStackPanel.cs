using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombatDicesTeam.Engine.Ui;

public sealed class BackgroundImage : ControlBase
{
    private readonly Point _textureOffset;
    private readonly Func<Color> _colorDelegate;

    public BackgroundImage(Texture2D texture, Point textureOffset, Func<Color> colorDelegate) : base(texture)
    {
        _textureOffset = textureOffset;
        _colorDelegate = colorDelegate;
    }

    protected override Point CalcTextureOffset()
    {
        return _textureOffset;
    }

    protected override Color CalculateColor()
    {
        return _colorDelegate();
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        
    }
}

public sealed class HorizontalStackPanel : ControlBase
{
    private readonly IReadOnlyList<ControlBase> _innerElements;
    private readonly Point _textureOffset;

    public HorizontalStackPanel(Texture2D texture, Point textureOffset, IReadOnlyList<ControlBase> innerElements) :
        base(texture)
    {
        _textureOffset = textureOffset;
        _innerElements = innerElements;
    }

    public override Point Size => _innerElements.Any()
        ? new Point(_innerElements.Sum(x => x.Size.X), _innerElements.Max(x => x.Size.Y))
        : Point.Zero;

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
        var currentPosition = contentRect.Location;
        foreach (var innerElement in _innerElements)
        {
            innerElement.Rect = new Rectangle(currentPosition, innerElement.Size);

            innerElement.Draw(spriteBatch);

            currentPosition += new Point(innerElement.Size.X, 0);
        }
    }
}