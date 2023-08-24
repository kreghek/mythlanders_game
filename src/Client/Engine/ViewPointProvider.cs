using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Client.Engine;

internal sealed class ViewPointProvider : IViewPointProvider
{
    private readonly IResolutionIndependentRenderer _resolutionIndependentRenderer;

    public ViewPointProvider(IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        _resolutionIndependentRenderer = resolutionIndependentRenderer;
    }

    public Vector2 GetWorldCoords()
    {
        var mouseState = Mouse.GetState();

        var worldMouse = _resolutionIndependentRenderer.ConvertScreenToWorldCoordinates(mouseState.Position.ToVector2());

        return worldMouse;
    }
}
