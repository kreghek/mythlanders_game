using System.Collections.Generic;
using System.Linq;

using Client.Engine;

using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat;

internal interface ICameraState
{
    bool IsComplete { get; }
    void Update(GameTime gameTime, ICamera2DAdapter camera);
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
    private readonly ICamera2DAdapter _camera;
    private readonly IList<ICameraState> _cameraStateList;

    private ICameraState _mainCameraState;

    public CameraOperator(ICamera2DAdapter camera, ICameraState startCameraState)
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