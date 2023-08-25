using Microsoft.Xna.Framework;

using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Client.Engine;

internal sealed class LayerCamera2DAdapter : ICamera2DAdapter
{
    private readonly ICamera2DAdapter _baseCamera;
    private Vector2 _position;
    private Vector2 _offset;

    public LayerCamera2DAdapter(ICamera2DAdapter baseCamera)
    {
        _baseCamera = baseCamera;
    }

    public Vector2 Offset
    {
        get
        {
            return _offset;
        }
        set
        {
            _offset = value;
            _baseCamera.Position = _position + value;
        }
    }

    public Vector2 Position
    {
        get { return _position; }
        set
        {
            _position = value;
            _baseCamera.Position = value + Offset;
        }
    }
    
    public float Zoom { get => _baseCamera.Zoom; set => _baseCamera.Zoom = value; }
    
    public Vector2 ConvertScreenToWorldCoordinates(Vector2 screenPosition) => _baseCamera.ConvertScreenToWorldCoordinates(screenPosition);

    public Matrix GetViewTransformationMatrix() => _baseCamera.GetViewTransformationMatrix();

    public void ZoomIn(float deltaZoom, Vector2 zoomCenter) => _baseCamera.ZoomIn(deltaZoom, zoomCenter);

    public void ZoomOut(float deltaZoom, Vector2 zoomCenter) => _baseCamera.ZoomOut(deltaZoom, zoomCenter);
}

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