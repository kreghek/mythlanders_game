using System;

using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

internal sealed class FollowActorOperatorCameraTask : ICameraOperatorTask
{
    private const float FOLLOWING_ZOOM = 2f;

    private const double ZOOM_THRESHOLD = 0.05;

    private const int ZOOM_SPEED = 10;
    private readonly IActorAnimator _combatActor;
    private readonly Func<bool> _completeDelegate;
    private Vector2 _lastActorPosition;

    public FollowActorOperatorCameraTask(IActorAnimator combatActor, Func<bool> completeDelegate)
    {
        _combatActor = combatActor;
        _completeDelegate = completeDelegate;
        _lastActorPosition = GetActorFollowPoint();
    }

    private Vector2 GetActorFollowPoint()
    {
        const int SPRITE_SIZE_Y = 128;
        return _combatActor.GraphicRoot.Position - new Vector2(0, SPRITE_SIZE_Y * 0.5f);
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
            var actorPositionDiff = _lastActorPosition - actorFollowPoint;
            camera.Position -= actorPositionDiff;
        }

        _lastActorPosition = actorFollowPoint;
    }
}