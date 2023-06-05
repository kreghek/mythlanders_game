using Microsoft.Xna.Framework;

using MonoGame.Extended.ViewportAdapters;

namespace Client.Engine;

internal interface IResolutionIndependentRenderer
{
    BoxingViewportAdapter ViewportAdapter { get; }
    Rectangle VirtualBounds { get; }
    void BeginDraw();

    /// <summary>
    /// Convert screen coordinates to coordinates in the world.
    /// </summary>
    /// <param name="screenPosition">Coordinates of screen. Example, mouse coordinates.</param>
    Vector2 ConvertScreenToWorldCoordinates(Vector2 screenPosition);

    void Initialize();
}