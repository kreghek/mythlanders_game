using System.Collections.Generic;
using System.Linq;

using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

internal class CameraOperator
{
    private readonly ICamera2DAdapter _ownedCamera;
    private readonly IList<ICameraOperatorTask> _taskList;

    private ICameraOperatorTask _mainCameraState;

    public CameraOperator(ICamera2DAdapter camera, ICameraOperatorTask startCameraState)
    {
        _ownedCamera = camera;

        _taskList = new List<ICameraOperatorTask>();
        _mainCameraState = startCameraState;
    }

    /// <summary>
    /// Add assign task to operator.
    /// </summary>
    public void AddState(ICameraOperatorTask task)
    {
        _taskList.Add(task);
    }

    /// <summary>
    /// Update camera operator state.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Update(GameTime gameTime)
    {
        HandleCameraStates(gameTime);
    }

    internal void SetMasterState(OverviewCameraState mainCameraState)
    {
        _taskList.Clear();

        _mainCameraState = mainCameraState;
    }

    private void HandleCameraStates(GameTime gameTime)
    {
        if (!_taskList.Any())
        {
            _mainCameraState.DoWork(gameTime, _ownedCamera);
            return;
        }

        var activeState = _taskList.First();
        activeState.DoWork(gameTime, _ownedCamera);

        if (activeState.IsComplete)
        {
            _taskList.Remove(activeState);

            if (!_taskList.Any())
            {
                _mainCameraState.DoWork(gameTime, _ownedCamera);
            }
        }
    }
}