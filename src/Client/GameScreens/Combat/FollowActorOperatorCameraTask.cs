using System;

using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

internal sealed class FollowActorOperatorCameraTask : ICameraOperatorTask
{
    private const float FOLLOWING_ZOOM = 1.3f;

    private const double ZOOM_THRESHOLD = 0.05;

    private const int ZOOM_SPEED = 3;
    private readonly IActorAnimator _combatActor;
    private readonly Func<bool> _completeDelegate;

    public FollowActorOperatorCameraTask(IActorAnimator combatActor, Func<bool> completeDelegate)
    {
        _combatActor = combatActor;
        _completeDelegate = completeDelegate;
    }

    private Vector2 GetActorFollowPoint()
    {
        const int SPRITE_SIZE_Y = 128;
        const int FIELD_WIDTH = 1000;
        const int MARGIN = 128 + 128 + 64;
        var actorViewPoint = _combatActor.GraphicRoot.Position - new Vector2(0, SPRITE_SIZE_Y * 0.5f);
        if (actorViewPoint.X < MARGIN)
        {
            return new Vector2(MARGIN, actorViewPoint.Y);
        }

        if (actorViewPoint.X > FIELD_WIDTH - MARGIN)
        {
            return new Vector2(FIELD_WIDTH - MARGIN, actorViewPoint.Y);
        }

        return actorViewPoint;
    }

    /// <inheritdoc />
    public bool IsComplete => _completeDelegate();

    /// <inheritdoc />
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
            camera.LookAt(actorFollowPoint);
        }
    }
}