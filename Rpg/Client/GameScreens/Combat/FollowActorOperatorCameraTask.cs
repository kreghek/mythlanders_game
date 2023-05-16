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
    
    /// <inheritdoc/>
    public void DoWork(GameTime gameTime, ICamera2DAdapter camera)
    {
        var spriteSizeY = 128;
        
        var actorFollowPoint = _combatActor.GraphicRoot.Position - new Vector2(0, spriteSizeY * 0.5f);
        
        if (Math.Abs(camera.Zoom - FOLLOWING_ZOOM) > 0.05)
        {
            if (camera.Zoom < FOLLOWING_ZOOM)
            {
                camera.ZoomIn((float)gameTime.ElapsedGameTime.TotalSeconds * 10, actorFollowPoint);
            }
            else if (camera.Zoom > FOLLOWING_ZOOM)
            {
                camera.ZoomOut((float)gameTime.ElapsedGameTime.TotalSeconds * 10, actorFollowPoint);
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
}