using Microsoft.Xna.Framework;

namespace GameClient.Engine.MoveFunctions;

/// <summary>
/// Function to speed up value in the end.
/// </summary>
public sealed class SpeedUpMoveFunction : IMoveFunction
{
    private readonly Vector2 _finishPosition;
    private readonly Vector2 _startPosition;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="startPosition">Start movement position.</param>
    /// <param name="finishPosition">Target movement position.</param>
    public SpeedUpMoveFunction(Vector2 startPosition, Vector2 finishPosition)
    {
        _startPosition = startPosition;
        _finishPosition = finishPosition;
    }

    /// <inheritdoc />
    public Vector2 CalcPosition(MoveFunctionArgument t)
    {
        return Vector2.Lerp(_startPosition, _finishPosition, (float)(1 - Math.Cos(t.Value * Math.PI * 0.5)));
    }
}