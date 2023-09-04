using Microsoft.Xna.Framework;

using MonoGame.Extended.ViewportAdapters;

namespace Client.Engine;

internal interface IScreenProjection
{
    /// <summary>
    /// Convert screen coordinates to coordinates in the world.
    /// </summary>
    /// <param name="screenPosition">Coordinates of screen. Example, mouse coordinates.</param>
    Vector2 ConvertScreenToWorldCoordinates(Vector2 screenPosition);
}

internal interface IResolutionIndependentRenderer: IScreenProjection
{
    BoxingViewportAdapter ViewportAdapter { get; }
    Rectangle VirtualBounds { get; }
    void BeginDraw();

    void Initialize();
}