using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Client.Engine;

public class ParallaxRectControl : RectControlBase
{
    private readonly Rectangle _screenRectange;
    private readonly Rectangle _layerRectangle;
    private readonly Vector2[] _speeds;
    private readonly IViewPointProvider _viewPointProvider;

    public ParallaxRectControl(Rectangle parentRectange,
        Rectangle layerRectangle,
        Vector2[] relativeSpeeds,
        IViewPointProvider viewPointProvider)
    {
        _screenRectange = parentRectange;
        _layerRectangle = layerRectangle;
        _speeds = relativeSpeeds;
        _viewPointProvider = viewPointProvider;
    }

    public override IReadOnlyList<Rectangle> GetRects()
    {
        var screenCenter = _screenRectange.Center;

        var worldMouse = _viewPointProvider.GetWorldCoords();

        var cursorDiff = worldMouse - screenCenter.ToVector2();

        var rects = new List<Rectangle>();

        for (var i = 0; i < _speeds.Length; i++)
        {
            var layerLocation = _layerRectangle.Center.ToVector2() * -1;
            var rectPosition = layerLocation - cursorDiff * _speeds[i];
            var rect = new Rectangle(rectPosition.ToPoint(), _screenRectange.Size);

            rects.Add(rect);
        }

        return rects;
    }
}
