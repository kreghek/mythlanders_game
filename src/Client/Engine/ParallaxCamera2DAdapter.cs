using GameClient.Engine.RectControl;

using Microsoft.Xna.Framework;

namespace Client.Engine;

internal sealed class ParallaxCamera2DAdapter : ICamera2DAdapter
{
    private readonly ICamera2DAdapter _mainCamera;
    private readonly ParallaxRectControl _parallaxRectControl;
    private readonly IResolutionIndependentRenderer _resolutionIndependentRenderer;
    private Vector2 _position;

    public ParallaxCamera2DAdapter(
        ParallaxRectControl parallaxRectControl,
        IResolutionIndependentRenderer resolutionIndependentRenderer,
        ICamera2DAdapter mainCamera,
        params ICamera2DAdapter[] layerCameras)
    {
        _parallaxRectControl = parallaxRectControl;
        _resolutionIndependentRenderer = resolutionIndependentRenderer;
        _mainCamera = mainCamera;
        LayerCameras = layerCameras;
    }

    internal ICamera2DAdapter[] LayerCameras { get; }

    public void Update()
    {
        var rects = _parallaxRectControl.GetRects();

        for (var i = 0; i < LayerCameras.Length; i++)
        {
            var layerCamera = LayerCameras[i];
            var layerDiff = (rects[i].Size - _resolutionIndependentRenderer.VirtualBounds.Size).ToVector2() / 2;
            layerCamera.Position = _position + rects[i].Center.ToVector2() + layerDiff;
        }
    }

    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;

            Update();
        }
    }

    public float Zoom
    {
        get => _mainCamera.Zoom;
        set => _mainCamera.Zoom = value;
    }

    public Vector2 ConvertScreenToWorldCoordinates(Vector2 screenPosition)
    {
        return _mainCamera.ConvertScreenToWorldCoordinates(screenPosition);
    }

    public Matrix GetViewTransformationMatrix()
    {
        return _mainCamera.GetViewTransformationMatrix();
    }

    public void ZoomIn(float deltaZoom, Vector2 zoomCenter)
    {
        _mainCamera.ZoomIn(deltaZoom, zoomCenter);
    }

    public void LookAt(Vector2 position)
    {
        _mainCamera.LookAt(position);
    }

    public void ZoomOut(float deltaZoom, Vector2 zoomCenter)
    {
        _mainCamera.ZoomOut(deltaZoom, zoomCenter);
    }
}