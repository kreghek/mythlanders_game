using Microsoft.Xna.Framework;

using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Client.Engine;

internal sealed class Camera2DAdapter : ICamera2DAdapter
{
    private readonly OrthographicCamera _innerCamera;

    public Camera2DAdapter(BoxingViewportAdapter viewportAdapter)
    {
        _innerCamera = new OrthographicCamera(viewportAdapter);
    }

    /// <summary>
    /// Center of the camera view.
    /// </summary>
    private Vector2 ViewCenter => _innerCamera.Origin;

    /// <summary>
    /// Camera position.
    /// </summary>
    public Vector2 Position
    {
        get => _innerCamera.Position;
        set => _innerCamera.Position = value;
    }

    /// <summary>
    /// Camera zoom.
    /// </summary>
    public float Zoom
    {
        get => _innerCamera.Zoom;
        set => _innerCamera.Zoom = value;
    }

    /// <summary>
    /// Zoom in camera.
    /// </summary>
    public void ZoomIn(float deltaZoom, Vector2 zoomCenter)
    {
        var pastZoom = Zoom;
        _innerCamera.ZoomIn(deltaZoom);
        Position += (zoomCenter - ViewCenter - Position) * ((Zoom - pastZoom) / Zoom);
    }

    /// <summary>
    /// Zoom out camera.
    /// </summary>
    public void ZoomOut(float deltaZoom, Vector2 zoomCenter)
    {
        var pastZoom = Zoom;
        _innerCamera.ZoomOut(deltaZoom);
        Position += (zoomCenter - ViewCenter - Position) * ((Zoom - pastZoom) / Zoom);
    }

    /// <summary>
    /// Returns transformation matrix of game objects.
    /// </summary>
    public Matrix GetViewTransformationMatrix()
    {
        return _innerCamera.GetViewMatrix();
    }

    /// <summary>
    /// Convert screen coordinates to coordinates in the world.
    /// </summary>
    /// <param name="screenPosition">Coordinates of screen. Example, mouse coordinates.</param>
    public Vector2 ConvertScreenToWorldCoordinates(Vector2 screenPosition)
    {
        return _innerCamera.ScreenToWorld(screenPosition);
    }
}