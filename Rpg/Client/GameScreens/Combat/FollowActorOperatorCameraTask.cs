using System;

using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

internal sealed class FollowActorOperatorCameraTask : ICameraOperatorTask
{
    private readonly IActorAnimator _combatActor;
    private readonly Func<bool> _completeDelegate;

    public FollowActorOperatorCameraTask(IActorAnimator combatActor, Func<bool> completeDelegate)
    {
        _combatActor = combatActor;
        _completeDelegate = completeDelegate;
    }

    /// <inheritdoc/>
    public bool IsComplete => _completeDelegate();

    const float FOLLOWING_ZOOM = 2f;

    private const double ZOOM_THRESHOLD = 0.05;

    private const int ZOOM_SPEED = 10;

    /// <inheritdoc/>
    public void DoWork(GameTime gameTime, ICamera2DAdapter camera)
    {
        var actorFollowPoint = GetActorFollowPoint();

        if (Math.Abs(camera.Zoom - FOLLOWING_ZOOM) > ZOOM_THRESHOLD)
        {
            if (camera.Zoom < FOLLOWING_ZOOM)
            {
                camera.ZoomIn((float)gameTime.ElapsedGameTime.TotalSeconds * ZOOM_SPEED, actorFollowPoint);
            }
            else if (camera.Zoom > FOLLOWING_ZOOM)
            {
                camera.ZoomOut((float)gameTime.ElapsedGameTime.TotalSeconds * ZOOM_SPEED, actorFollowPoint);
            }
            else
            {
                camera.Zoom = FOLLOWING_ZOOM;
            }
        }
        else
        {
            camera.Zoom = FOLLOWING_ZOOM;
            camera.ZoomIn(0.01f, actorFollowPoint);
        }
    }

    private Vector2 GetActorFollowPoint()
    {
        const int SPRITE_SIZE_Y = 128;
        return _combatActor.GraphicRoot.Position - new Vector2(0, SPRITE_SIZE_Y * 0.5f);
    }
}