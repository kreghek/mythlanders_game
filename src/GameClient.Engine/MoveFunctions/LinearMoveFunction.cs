using Microsoft.Xna.Framework;

namespace GameClient.Engine.MoveFunctions;

/// <summary>
/// Function to move position using constant speed.
/// </summary>
public sealed class LinearMoveFunction : IMoveFunction
{
    private readonly Vector2 _finishPosition;
    private readonly Vector2 _startPosition;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="startPosition">Start movement position.</param>
    /// <param name="finishPosition">Target movement position.</param>
    public LinearMoveFunction(Vector2 startPosition, Vector2 finishPosition)
    {
        _startPosition = startPosition;
        _finishPosition = finishPosition;
    }

    /// <inheritdoc />
    public Vector2 CalcPosition(MoveFunctionArgument t)
    {
        return Vector2.Lerp(_startPosition, _finishPosition, (float)t.Value);
    }
}