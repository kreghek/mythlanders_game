using System;

using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

internal sealed class OverviewCameraOperatorTask : ICameraOperatorTask
{
    private readonly Vector2 _overviewPosition;
    private readonly Vector2 _overviewOrigin;


    public OverviewCameraOperatorTask(Vector2 overviewPosition)
    {
        _overviewPosition = overviewPosition;
        _overviewOrigin = _overviewPosition + new Vector2(848 / 2f, 480 / 2f);
    }

    /// <inheritdoc/>
    public bool IsComplete => false;

    private const float BASE_ZOOM = 1.0f;
    
    private const double ZOOM_THRESHOLD = 0.05;
    
    private const int ZOOM_SPEED = 10;

    /// <inheritdoc/>
    public void DoWork(GameTime gameTime, ICamera2DAdapter camera)
    {
        if (Math.Abs(camera.Zoom - BASE_ZOOM) > ZOOM_THRESHOLD)
        {
            if (camera.Zoom < BASE_ZOOM)
            {
                camera.ZoomIn((float)gameTime.ElapsedGameTime.TotalSeconds * ZOOM_SPEED, _overviewOrigin);
            }
            else if (camera.Zoom > BASE_ZOOM)
            {
                camera.ZoomOut((float)gameTime.ElapsedGameTime.TotalSeconds * ZOOM_SPEED, _overviewOrigin);
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
                camera.Position = Vector2.Lerp(camera.Position, _overviewPosition,
                        (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }
}