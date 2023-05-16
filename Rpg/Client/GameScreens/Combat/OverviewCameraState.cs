using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat;

internal sealed class OverviewCameraState: ICameraState
{
    private readonly Vector2 _overviewPosition;
    private readonly Vector2 _overviewOrigin;


    public OverviewCameraState(Vector2 overviewPosition)
    {
        _overviewPosition = overviewPosition;
        _overviewOrigin = _overviewPosition + new Vector2(848 / 2f, 480 / 2f);
    }

    private static float Lerp(float a, float b, float t)
    {
        return a * (1 - t) + b * t;
    }

    public bool IsComplete => false;

    private const float BASE_ZOOM = 1.0f;
    
    public void Update(GameTime gameTime, Camera2D camera)
    {
        if (Math.Abs(camera.Zoom - BASE_ZOOM) > 0.05)
        {

            if (camera.Zoom < BASE_ZOOM)
            {
                camera.ZoomIn((float)gameTime.ElapsedGameTime.TotalSeconds * 10, _overviewOrigin);
            }
            else if (camera.Zoom > BASE_ZOOM)
            {
                camera.ZoomOut((float)gameTime.ElapsedGameTime.TotalSeconds * 10, _overviewOrigin);
            }
            else
            {
                camera.Zoom = BASE_ZOOM;
            }
        }
        else
        {
            camera.Zoom = BASE_ZOOM;
            var distance = (camera.Position - _overviewPosition).Length();
            if (distance > 0.1f)
            {
                camera.Position = Vector2.Lerp(camera.Position, _overviewPosition, (float)gameTime.ElapsedGameTime.TotalSeconds)
                    /*.ToIntVector2()*/;
             }
        }
    }
}