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

    const float FOLLOWING_ZOOM = 1.5f;
    
    /// <inheritdoc/>
    public void Update(GameTime gameTime, Camera2D camera)
    {
        //camera.Zoom = Lerp(camera.Zoom, FOLLOWING_ZOOM, (float)gameTime.ElapsedGameTime.TotalSeconds * 2);
        
        var spriteSizeY = 128;
        
        var actorFollowPoint = _combatActor.GraphicRoot.Position - new Vector2(0, spriteSizeY * 0.5f);
        //camera.LookAt(Vector2.Lerp(camera.Position, actorFollowPoint,            (float)gameTime.ElapsedGameTime.TotalSeconds * 5f));
        camera.LookAt(_combatActor.GraphicRoot.Position);
    }
}