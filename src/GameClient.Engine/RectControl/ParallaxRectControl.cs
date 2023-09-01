using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

public class ParallaxRectControl : RectControlBase
{
    private readonly Rectangle _screenRectangle;
    private readonly Rectangle _layerRectangle;
    private readonly Vector2[] _speeds;
    private readonly IViewPointProvider _viewPointProvider;

    public ParallaxRectControl(Rectangle parentRectangle,
        Rectangle layerRectangle,
        Vector2[] relativeSpeeds,
        IViewPointProvider viewPointProvider)
    {
        _screenRectangle = parentRectangle;
        _layerRectangle = layerRectangle;
        _speeds = relativeSpeeds;
        _viewPointProvider = viewPointProvider;
    }

    public override IReadOnlyList<Rectangle> GetRects()
    {
        return _speeds.Select(speed => CreateRectangle(speed)).ToArray();
    }

    private Rectangle CreateRectangle(Vector2 speed)
    {
        var screenCenter = _screenRectangle.Center;

        var worldMouse = _viewPointProvider.GetWorldCoords();

        var cursorDiff = worldMouse - screenCenter.ToVector2();
        
        var layerStartLocation = _layerRectangle.Center.ToVector2() * -1;
        var layerOffset = cursorDiff * -speed;
        var layerOffsetPosition = layerStartLocation + layerOffset;
        var rect = new Rectangle(layerOffsetPosition.ToPoint(), _layerRectangle.Size);
        return rect;
    }
}
