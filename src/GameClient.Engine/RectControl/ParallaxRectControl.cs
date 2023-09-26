using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

/// <summary>
/// Rect control to move multiple rects using parallax effect.
/// </summary>
public class ParallaxRectControl : RectControlBase
{
    private readonly Point[] _layerSizes;
    private readonly Rectangle _screenRectangle;
    private readonly IParallaxViewPointProvider _viewPointProvider;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="parentRectangle"> Parent rectangle of inner layers. </param>
    /// <param name="layerSizes"> Parallax layers. </param>
    /// <param name="viewPointProvider"> Provider to get the view point required for parallax effect. </param>
    public ParallaxRectControl(Rectangle parentRectangle,
        Point[] layerSizes,
        IParallaxViewPointProvider viewPointProvider)
    {
        _screenRectangle = parentRectangle;
        _layerSizes = layerSizes;
        _viewPointProvider = viewPointProvider;
    }

    /// <inheritdoc cref="RectControlBase.GetRects" />
    public override IReadOnlyList<Rectangle> GetRects()
    {
        return _layerSizes.Select(CreateRectangle).ToArray();
    }

    private Rectangle CreateRectangle(Point layerSize)
    {
        var worldMouse = _viewPointProvider.GetWorldCoords();

        var t = new Vector2(worldMouse.X / _screenRectangle.Width, worldMouse.Y / _screenRectangle.Height);

        var xOffset = (int)MathHelper.Lerp(0, layerSize.X / 2, t.X);
        var yOffset = (int)MathHelper.Lerp(0, layerSize.Y / 2, t.Y);

        var rectPosition = new Vector2(-(xOffset), -(yOffset));

        return new Rectangle(
            rectPosition.ToPoint(),
            layerSize);
    }
}