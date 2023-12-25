using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

/// <summary>
/// Rect control to move inner rect into the parent using direction.
/// </summary>
public sealed class PongRectangleControl : RectControlBase
{
    private readonly IPongRectangleDirectionProvider _directionProvider;
    private readonly Rectangle _parentRectangle;
    private readonly Point _size;

    private Vector2 _bgCurrentPosition;

    private Vector2 _bgMoveVector;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="size"> Size of inner rect. </param>
    /// <param name="parentRectangle"> Parent rectangle to move inner rect inside. </param>
    /// <param name="directionProvider"> Provider of direction. </param>
    public PongRectangleControl(Point size, Rectangle parentRectangle,
        IPongRectangleDirectionProvider directionProvider)
    {
        _size = size;
        _parentRectangle = parentRectangle;
        _directionProvider = directionProvider;

        _bgMoveVector = directionProvider.GetNextVector();
    }

    /// <inheritdoc />
    public override IReadOnlyList<Rectangle> GetRects()
    {
        return new[] { new Rectangle(_bgCurrentPosition.ToPoint(), _size) };
    }

    /// <summary>
    /// Update the state of inner rect.
    /// </summary>
    /// <param name="timeElapsedSeconds"> Time elapsed from previous update. </param>
    public void Update(double timeElapsedSeconds)
    {
        var nextRect = new Rectangle((_bgCurrentPosition + _bgMoveVector).ToPoint(), _size);

        if (nextRect.Contains(_parentRectangle))
        {
            _bgCurrentPosition += _bgMoveVector * (float)timeElapsedSeconds;
        }
        else
        {
            _bgMoveVector = _directionProvider.GetNextVector();
        }
    }
}