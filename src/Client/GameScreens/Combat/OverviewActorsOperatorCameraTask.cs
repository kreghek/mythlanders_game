using System;

using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

internal sealed class OverviewActorsOperatorCameraTask : ICameraOperatorTask
{
    private const double ZOOM_THRESHOLD = 0.05;

    private const float ZOOM_SPEED = 1.5f;
    private readonly IActorAnimator _combatActor;
    private readonly Func<bool> _completeDelegate;
    private readonly IActorAnimator _targetActor;
    private readonly float _targetZoom;

    public OverviewActorsOperatorCameraTask(IActorAnimator combatActor, IActorAnimator targetActor, float targetZoom,
        Func<bool> completeDelegate)
    {
        _combatActor = combatActor;
        _targetActor = targetActor;

        _targetZoom = targetZoom;
        _completeDelegate = completeDelegate;
    }

    private Vector2 GetActorFollowPoint(IActorAnimator _combatActor)
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
        var actorFollowPoint = GetActorFollowPoint(_combatActor);

        var targetFollowPoint = GetActorFollowPoint(_targetActor);

        var betweenFollowPoint = (targetFollowPoint - actorFollowPoint) * 0.5f + actorFollowPoint;

        if (Math.Abs(camera.Zoom - _targetZoom) > ZOOM_THRESHOLD)
        {
            if (camera.Zoom < _targetZoom)
            {
                camera.ZoomIn((float)gameTime.ElapsedGameTime.TotalSeconds * ZOOM_SPEED, betweenFollowPoint);
            }
            else if (camera.Zoom > _targetZoom)
            {
                camera.ZoomOut((float)gameTime.ElapsedGameTime.TotalSeconds * ZOOM_SPEED, betweenFollowPoint);
            }
            else
            {
                camera.Zoom = _targetZoom;
            }
        }
        else
        {
            camera.Zoom = _targetZoom;
            camera.LookAt(betweenFollowPoint);
        }
    }
}