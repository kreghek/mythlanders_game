using System;

using Microsoft.Xna.Framework;

namespace Client.Engine.MoveFunctions;

public sealed class SlowDownMoveFunction : IMoveFunction
{
    private readonly Vector2 _finishPosition;
    private readonly Vector2 _startPosition;

    public SlowDownMoveFunction(Vector2 startPosition, Vector2 finishPosition)
    {
        _startPosition = startPosition;
        _finishPosition = finishPosition;
    }

    public Vector2 CalcPosition(double t)
    {
        return Vector2.Lerp(_startPosition, _finishPosition, (float)Math.Sin(t * Math.PI * 0.5));
    }
}