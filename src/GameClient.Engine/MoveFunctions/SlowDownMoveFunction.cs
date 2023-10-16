using Microsoft.Xna.Framework;

namespace GameClient.Engine.MoveFunctions;

/// <summary>
/// Function to slow down value in the end.
/// </summary>
public sealed class SlowDownMoveFunction : IMoveFunction
{
    private readonly Vector2 _finishPosition;
    private readonly Vector2 _startPosition;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="startPosition">Start movement position.</param>
    /// <param name="finishPosition">Target movement position.</param>
    public SlowDownMoveFunction(Vector2 startPosition, Vector2 finishPosition)
    {
        _startPosition = startPosition;
        _finishPosition = finishPosition;
    }

    /// <inheritdoc />
    public Vector2 CalcPosition(MoveFunctionValue t)
    {
        return Vector2.Lerp(_startPosition, _finishPosition, (float)Math.Sin(t.Value * Math.PI * 0.5));
    }
}