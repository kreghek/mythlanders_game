using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombatDicesTeam.Engine.Ui;

public sealed class HorizontalStackPanel : UiElementContentBase
{
    private readonly IReadOnlyList<UiElementContentBase> _innerElements;
    private readonly Point _textureOffset;

    public HorizontalStackPanel(Texture2D texture, Point textureOffset, IReadOnlyList<UiElementContentBase> innerElements) :
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
            innerElement.Rect = new Rectangle(currentPosition, innerElement.Rect.Size);

            innerElement.Draw(spriteBatch);

            currentPosition += new Point(innerElement.Size.X, 0);
        }
    }
}