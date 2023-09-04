using Microsoft.Xna.Framework;

namespace Client.Engine;

/// <summary>
/// Service to work with projection of the screen projection of the game-world objects. 
/// </summary>
internal interface IScreenProjection
{
    /// <summary>
    /// Convert screen coordinates to coordinates in the world.
    /// </summary>
    /// <param name="screenPosition">Coordinates of screen. Example, mouse coordinates.</param>
    Vector2 ConvertScreenToWorldCoordinates(Vector2 screenPosition);
}