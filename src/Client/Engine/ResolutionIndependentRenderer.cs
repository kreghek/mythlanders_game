using Microsoft.Xna.Framework;

using MonoGame.Extended.ViewportAdapters;

namespace Client.Engine;

internal class ResolutionIndependentRenderer : IResolutionIndependentRenderer
{
    private readonly ICamera2DAdapter _camera;

    public int VirtualHeight;

    public int VirtualWidth;

    public ResolutionIndependentRenderer(ICamera2DAdapter camera, BoxingViewportAdapter viewportAdapter)
    {
        _camera = camera;
        ViewportAdapter = viewportAdapter;
        VirtualWidth = 1366;
        VirtualHeight = 768;
    }

    public Rectangle VirtualBounds => ViewportAdapter.BoundingRectangle;

    public BoxingViewportAdapter ViewportAdapter { get; }

    public void BeginDraw()
    {
    }


    public void Initialize()
    {
    }

    /// <summary>
    /// Convert screen coordinates to coordinates in the world.
    /// </summary>
    /// <param name="screenPosition">Coordinates of screen. Example, mouse coordinates.</param>
    public Vector2 ConvertScreenToWorldCoordinates(Vector2 screenPosition)
    {
        return _camera.ConvertScreenToWorldCoordinates(screenPosition);
    }
}