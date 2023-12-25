using System;

using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

internal sealed class OverviewCameraOperatorTask : ICameraOperatorTask
{
    private const float BASE_ZOOM = 1.0f;

    private const double ZOOM_THRESHOLD = 0.05;

    private const int ZOOM_SPEED = 10;
    private readonly Func<Vector2> _overviewPositionFunc;


    public OverviewCameraOperatorTask(Func<Vector2> overviewPositionFunc)
    {
        _overviewPositionFunc = overviewPositionFunc;
    }

    /// <inheritdoc />
    public bool IsComplete => false;

    /// <inheritdoc />
    public void DoWork(GameTime gameTime, ICamera2DAdapter camera)
    {
        if (Math.Abs(camera.Zoom - BASE_ZOOM) > ZOOM_THRESHOLD)
        {
            if (camera.Zoom < BASE_ZOOM)
            {
                camera.ZoomIn((float)gameTime.ElapsedGameTime.TotalSeconds * ZOOM_SPEED, _overviewPositionFunc());
            }
            else if (camera.Zoom > BASE_ZOOM)
            {
                camera.ZoomOut((float)gameTime.ElapsedGameTime.TotalSeconds * ZOOM_SPEED, _overviewPositionFunc());
            }
            else
            {
                camera.Zoom = BASE_ZOOM;
            }
        }
        else
        {
            camera.LookAt(_overviewPositionFunc());
        }
    }
}