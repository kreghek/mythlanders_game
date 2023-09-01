using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

public sealed class PongRectangleControl: RectControlBase
{
    private readonly Rectangle _parentRectangle;
    private readonly IPongRectangleRandomSource _randomSource;
    private readonly Point _size;

    private Vector2 _bgCurrentPosition;

    private Vector2 _bgMoveVector;

    public PongRectangleControl(Point size, Rectangle parentRectangle, IPongRectangleRandomSource randomSource)
    {
        _size = size;
        _parentRectangle = parentRectangle;
        _randomSource = randomSource;

        _bgMoveVector = randomSource.GetRandomVector();
    }

    public override IReadOnlyList<Rectangle> GetRects()
    {
        return new[] { new Rectangle(_bgCurrentPosition.ToPoint(), _size) };
    }

    public void Update(double timeElapsedSeconds)
    {
        var nextRect = new Rectangle((_bgCurrentPosition + _bgMoveVector).ToPoint(), _size);

        if (nextRect.Contains(_parentRectangle))
        {
            _bgCurrentPosition += _bgMoveVector * (float)timeElapsedSeconds;
        }
        else
        {
            _bgMoveVector = _randomSource.GetRandomVector();
        }
    }
}