using Microsoft.Xna.Framework;

namespace GameClient.Engine.MoveFunctions;

/// <summary>
/// Function to move vector from 0 .. 1.
/// </summary>
public interface IMoveFunction
{
    /// <summary>
    /// Gets vector by specified t value.
    /// </summary>
    /// <param name="t">Function value 0..1</param>
    /// <returns>Return vect2 position.</returns>
    Vector2 CalcPosition(MoveFunctionArgument t);
}