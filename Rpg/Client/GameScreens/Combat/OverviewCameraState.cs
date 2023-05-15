using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat;

internal sealed class OverviewCameraState: ICameraState
{
    private readonly Vector2 _overviewPosition;
    
    
    public OverviewCameraState(Vector2 overviewPosition)
    {
        _overviewPosition = overviewPosition;
    }

    private static float Lerp(float a, float b, float t)
    {
        return a * (1 - t) + b * t;
    }

    public bool IsComplete => false;

    public void Update(GameTime gameTime, Camera2D camera)
    {
        const float BASE_ZOOM = 2f;

        if (camera.Zoom < BASE_ZOOM)
        {
            camera.ZoomIn((float)gameTime.ElapsedGameTime.TotalSeconds * 10, camera.Origin);
        }

        var distance = (camera.Position - _overviewPosition).Length();
        if (distance > 0.1f)
        {
            camera.Position = Vector2.Lerp(camera.Position, _overviewPosition, (float)gameTime.ElapsedGameTime.TotalSeconds)
                /*.ToIntVector2()*/;
        }
    }
}