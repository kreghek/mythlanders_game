using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine;

public sealed class VerticalStackPanel: ControlBase
{
    private readonly Point _textureOffset;
    private readonly IReadOnlyList<ControlBase> _innerElements;

    public VerticalStackPanel(Texture2D texture, Point textureOffset, IReadOnlyList<ControlBase> innerElements) : base(texture)
    {
        _textureOffset = textureOffset;
        _innerElements = innerElements;
    }

    protected override Point CalcTextureOffset() => _textureOffset;

    protected override Color CalculateColor() => Color.White;

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        var currentPosition = contentRect.Location;
        for (var elementIndex = 0; elementIndex < _innerElements.Count; elementIndex++)
        {
            var innerElement = _innerElements[elementIndex];
            innerElement.Rect = new Rectangle(currentPosition, innerElement.Rect.Size);
            
            innerElement.Draw(spriteBatch);

            currentPosition += new Point(0, innerElement.Rect.Size.Y);
        }
    }
}