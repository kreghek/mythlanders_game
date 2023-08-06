using Microsoft.Xna.Framework;

namespace Client.Engine.MoveFunctions;

public sealed class LinearMoveFunction : IMoveFunction
{
    private readonly Vector2 _finishPosition;
    private readonly Vector2 _startPosition;

    public LinearMoveFunction(Vector2 startPosition, Vector2 finishPosition)
    {
        _startPosition = startPosition;
        _finishPosition = finishPosition;
    }

    public Vector2 CalcPosition(double t)
    {
        return Vector2.Lerp(_startPosition, _finishPosition, (float)t);
    }
}