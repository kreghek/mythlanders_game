using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

/// <summary>
/// Provider of view point in-world coordinates to move parallax layers.
/// </summary>
public interface IParallaxViewPointProvider
{
    /// <summary>
    /// Gets in-world coordinates.
    /// </summary>
    /// <returns> Coordinates in game world. </returns>
    Vector2 GetWorldCoords();
}