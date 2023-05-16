using System;

using Client.Engine;

using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat;

internal sealed class FollowActorCameraState : ICameraState
{
    private readonly IActorAnimator _combatActor;
    private readonly Func<bool> _completeDelegate;

    public FollowActorCameraState(IActorAnimator combatActor, Func<bool> completeDelegate)
    {
        _combatActor = combatActor;
        _completeDelegate = completeDelegate;
    }

    private static float Lerp(float a, float b, float t)
    {
        return a * (1 - t) + b * t;
    }

    public bool IsComplete => _completeDelegate();

    const float FOLLOWING_ZOOM = 2f;
    
    /// <inheritdoc/>
    public void Update(GameTime gameTime, Camera2D camera)
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