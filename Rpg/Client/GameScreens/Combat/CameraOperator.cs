using System;
using System.Collections.Generic;
using System.Linq;

using Client.Engine;

using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat;

internal interface ICameraState
{
    bool IsComplete { get; }
    void Update(GameTime gameTime, Camera2D camera);
}

internal sealed class OverviewCameraState: ICameraState
{
    private readonly Vector2 _overviewPosition;
    
    
    public OverviewCameraState(Vector2 overviewPosition)
    {
        _overviewPosition = overviewPosition;
    }

    private static float Lerp(float a, float b, float t)
    {
        return a * (1 - t) + b * t;
    }

    public bool IsComplete => false;

    public void Update(GameTime gameTime, Camera2D camera)
    {
        camera.Zoom = Lerp(camera.Zoom, 1f, (float)gameTime.ElapsedGameTime.TotalSeconds * 10);

        var distance = (camera.Position - _overviewPosition).Length();
        if (distance > 0.1f)
        {
            camera.Position = Vector2.Lerp(camera.Position, _overviewPosition, (float)gameTime.ElapsedGameTime.TotalSeconds)
                .ToIntVector2();
        }
        
        camera.RecalculateTransformationMatrices();
    }
}

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

    public void Update(GameTime gameTime, Camera2D camera)
    {
        camera.Zoom = Lerp(camera.Zoom, 2f, (float)gameTime.ElapsedGameTime.TotalSeconds * 2);
        camera.Position = Vector2.Lerp(camera.Position, _combatActor.GraphicRoot.Position,
            (float)gameTime.ElapsedGameTime.TotalSeconds * 5f).ToIntVector2();
        
        camera.RecalculateTransformationMatrices();
    }
}

public static class Vector2Extensions
{
    public static Vector2 ToIntVector2(this Vector2 source)
    {
        return new Vector2((int)source.X, (int)source.Y);
    }
}

internal class CameraOperator
{
    private readonly Camera2D _camera;
    private readonly IList<ICameraState> _cameraStateList;

    private ICameraState _mainCameraState;

    public CameraOperator(Camera2D camera, ICameraState startCameraState)
    {
        _camera = camera;

        _cameraStateList = new List<ICameraState>();
        _mainCameraState = startCameraState;
    }

    public void AddState(ICameraState actorState)
    {
        _cameraStateList.Add(actorState);
    }

    public void Update(GameTime gameTime)
    {
        HandleCameraStates(gameTime);
    }

    internal void SetMasterState(OverviewCameraState mainCameraState)
    {
        _cameraStateList.Clear();

        _mainCameraState = mainCameraState;
    }

    private void HandleCameraStates(GameTime gameTime)
    {
        if (!_cameraStateList.Any())
        {
            _mainCameraState.Update(gameTime, _camera);
            return;
        }

        var activeState = _cameraStateList.First();
        activeState.Update(gameTime, _camera);

        if (activeState.IsComplete)
        {
            _cameraStateList.Remove(activeState);

            if (!_cameraStateList.Any())
            {
                _mainCameraState.Update(gameTime, _camera);
            }
        }
    }
}