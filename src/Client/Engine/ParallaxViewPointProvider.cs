using GameClient.Engine.RectControl;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Client.Engine;

internal sealed class ParallaxViewPointProvider : IParallaxViewPointProvider
{
    private readonly IResolutionIndependentRenderer _resolutionIndependentRenderer;

    public ParallaxViewPointProvider(IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        _resolutionIndependentRenderer = resolutionIndependentRenderer;
    }

    public Vector2 GetWorldCoords()
    {
        var mouseState = Mouse.GetState();

        var worldMouse =
            _resolutionIndependentRenderer.ConvertScreenToWorldCoordinates(mouseState.Position.ToVector2());

        return worldMouse;
    }
}