using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

/// <summary>
/// Rect control to move multiple rects using parallax effect.
/// </summary>
public class ParallaxRectControl : RectControlBase
{
    private readonly Rectangle[] _layerRectangles;
    private readonly Rectangle _screenRectangle;
    private readonly IParallaxViewPointProvider _viewPointProvider;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="parentRectangle"> Parent rectangle of inner layers. </param>
    /// <param name="layerRectangles"> Parallax layers. </param>
    /// <param name="viewPointProvider"> Provider to get the view point required for parallax effect. </param>
    public ParallaxRectControl(Rectangle parentRectangle,
        Rectangle[] layerRectangles,
        IParallaxViewPointProvider viewPointProvider)
    {
        _screenRectangle = parentRectangle;
        _layerRectangles = layerRectangles;
        _viewPointProvider = viewPointProvider;
    }

    /// <inheritdoc cref="RectControlBase.GetRects" />
    public override IReadOnlyList<Rectangle> GetRects()
    {
        return _layerRectangles.Select(CreateRectangle).ToArray();
    }

    private Rectangle CreateRectangle(Rectangle layerRectangle)
    {
        var worldMouse = _viewPointProvider.GetWorldCoords();

        var t = new Vector2(worldMouse.X / _screenRectangle.Width, worldMouse.Y / _screenRectangle.Height);

        var xOffset = (int)MathHelper.Lerp(0, layerRectangle.Width - layerRectangle.Center.X, t.X);
        var yOffset = (int)MathHelper.Lerp(0, layerRectangle.Height - layerRectangle.Center.Y, t.Y);

        var rectPosition = new Vector2(-(xOffset), -(yOffset));

        return new Rectangle(
            rectPosition.ToPoint(),
            layerRectangle.Size);
    }
}