using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

/// <summary>
/// Provider of random one-length vector to calculate next movement vector.  
/// </summary>
public interface IPongRectangleDirectionProvider
{
    /// <summary>
    /// Calculate next vector of inner rectangle.
    /// </summary>
    /// <returns> One-length vector of inner rect movement direction. </returns>
    Vector2 GetNextVector();
}