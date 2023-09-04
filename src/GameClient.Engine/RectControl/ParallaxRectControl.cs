using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

/// <summary>
/// Rect control to move multiple rects using parallax effect.
/// </summary>
public class ParallaxRectControl : RectControlBase
{
    private readonly Rectangle _screenRectangle;
    private readonly Rectangle _layerRectangle;
    private readonly Vector2[] _speeds;
    private readonly IParallaxViewPointProvider _viewPointProvider;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="parentRectangle"> Parent rectangle of inner layers. </param>
    /// <param name="layerRectangle"> Layer rectangle to calculate start position and size of ALL layers. </param>
    /// <param name="relativeSpeeds"> Parallax speeds of layers. </param>
    /// <param name="viewPointProvider"> Provider to get the view point required for parallax effect. </param>
    public ParallaxRectControl(Rectangle parentRectangle,
        Rectangle layerRectangle,
        Vector2[] relativeSpeeds,
        IParallaxViewPointProvider viewPointProvider)
    {
        _screenRectangle = parentRectangle;
        _layerRectangle = layerRectangle;
        _speeds = relativeSpeeds;
        _viewPointProvider = viewPointProvider;
    }

    /// <inheritdoc cref="RectControlBase.GetRects"/>
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
