using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Client.Engine;

public class ParallaxRectControl : RectControlBase
{
    private readonly Rectangle _screenRectange;
    private readonly Rectangle _layerRectangle;
    private readonly Vector2[] _speeds;
    private readonly int _mainLayerIndex;
    private readonly IViewPointProvider _viewPointProvider;

    public ParallaxRectControl(Rectangle parentRectange,
        Rectangle layerRectangle,
        Vector2[] relativeSpeeds,
        int baseLayerIndex,
        IViewPointProvider viewPointProvider)
    {
        _screenRectange = parentRectange;
        _layerRectangle = layerRectangle;
        _speeds = relativeSpeeds;
        _mainLayerIndex = baseLayerIndex;
        _viewPointProvider = viewPointProvider;
    }

    public override IReadOnlyList<Rectangle> GetRects()
    {
        var screenCenter = _screenRectange.Center;

        var worldMouse = _viewPointProvider.GetWorldCoords();

        var cursorDiff = worldMouse - screenCenter.ToVector2();

        var rects = new List<Rectangle>();

        var absoluteSpeeds = CalculateSpeeds(_speeds, _mainLayerIndex);

        var layerDiff = _layerRectangle.Size - _screenRectange.Size;

        for (var i = 0; i < absoluteSpeeds.Length; i++)
        {
            var layerLocation = _layerRectangle.Center.ToVector2() * -1;
            var layerCenter = new Rectangle(layerLocation.ToPoint(), _layerRectangle.Size).Center;
            var rectPosition = layerLocation - cursorDiff * absoluteSpeeds[i];
            var rect = new Rectangle(rectPosition.ToPoint(), _screenRectange.Size);

            rects.Add(rect);
        }

        return rects;
    }

    private static Vector2[] CalculateSpeeds(Vector2[] speeds, int baseLayerIndex)
    {
        var calculatedSpeeds = new Vector2[speeds.Length];

        calculatedSpeeds[baseLayerIndex] = speeds[baseLayerIndex];

        for (var i = baseLayerIndex + 1; i < speeds.Length; i++)
        {
            calculatedSpeeds[i] = calculatedSpeeds[i -1] + speeds[i];
        }

        return calculatedSpeeds;
    }
}
