using Microsoft.Xna.Framework;

using MonoGame.Extended.ViewportAdapters;

namespace Client.Engine;

internal interface IResolutionIndependentRenderer
{
    Rectangle VirtualBounds { get; }
    BoxingViewportAdapter ViewportAdapter { get; }
    void BeginDraw();
    void Initialize();

    /// <summary>
    /// Convert screen coordinates to coordinates in the world.
    /// </summary>
    /// <param name="screenPosition">Coordinates of screen. Example, mouse coordinates.</param>
    Vector2 ConvertScreenToWorldCoordinates(Vector2 screenPosition);
}