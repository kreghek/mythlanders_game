using Microsoft.Xna.Framework;

namespace Client.Engine;

internal sealed class ParallaxCamera2DAdapter : ICamera2DAdapter
{
    private readonly ParallaxRectControl _parallaxRectControl;
    private readonly ICamera2DAdapter _mainCamera;
    private Vector2 _position;

    public ParallaxCamera2DAdapter(ParallaxRectControl parallaxRectControl, ICamera2DAdapter mainCamera, params ICamera2DAdapter[] layerCameras)
    {
        _parallaxRectControl = parallaxRectControl;
        _mainCamera = mainCamera;
        LayerCameras = layerCameras;
    }

    public Vector2 Position
    {
        get { return _position; }
        set
        {
            _position = value;

            Update();
        }
    }

    public void Update()
    {
        var rects = _parallaxRectControl.GetRects();

        for (var i = 0; i < LayerCameras.Length; i++)
        {
            var layerCamera = LayerCameras[i];
            layerCamera.Position = _position + rects[i].Location.ToVector2() + (rects[i].Size.ToVector2() / 2);
        }
    }
    
    public float Zoom
    {
        get => _mainCamera.Zoom; 
        set
        {
            _mainCamera.Zoom = value;
        }
    }

    internal ICamera2DAdapter[] LayerCameras { get; }

    public Vector2 ConvertScreenToWorldCoordinates(Vector2 screenPosition) => _mainCamera.ConvertScreenToWorldCoordinates(screenPosition);

    public Matrix GetViewTransformationMatrix() => _mainCamera.GetViewTransformationMatrix();

    public void ZoomIn(float deltaZoom, Vector2 zoomCenter) => _mainCamera.ZoomIn(deltaZoom, zoomCenter);

    public void ZoomOut(float deltaZoom, Vector2 zoomCenter) => _mainCamera.ZoomOut(deltaZoom, zoomCenter);
}
