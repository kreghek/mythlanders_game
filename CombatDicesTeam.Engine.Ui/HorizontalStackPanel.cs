using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombatDicesTeam.Engine.Ui;

public sealed class HorizontalStackPanel: ControlBase
{
    private readonly Point _textureOffset;
    private readonly IReadOnlyList<ControlBase> _innerElements;

    public HorizontalStackPanel(Texture2D texture, Point textureOffset, IReadOnlyList<ControlBase> innerElements) : base(texture)
    {
        _textureOffset = textureOffset;
        _innerElements = innerElements;
    }

    protected override Point CalcTextureOffset() => _textureOffset;

    protected override Color CalculateColor() => Color.White;

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

    public override Point Size => _innerElements.Any() ? new Point(_innerElements.Sum(x => x.Size.X), _innerElements.Max(x => x.Size.Y)) : Point.Zero;

    protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
    {
        // No background
    }
}