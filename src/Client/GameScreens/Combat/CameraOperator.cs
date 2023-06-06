using System.Collections.Generic;
using System.Linq;

using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

internal class CameraOperator
{
    private readonly ICameraOperatorTask _idleTask;
    private readonly ICamera2DAdapter _ownedCamera;
    private readonly IList<ICameraOperatorTask> _taskList;

    /// <summary>
    /// Construct the operator.
    /// </summary>
    /// <param name="camera">Owned camera to operate.</param>
    /// <param name="idleTask">Task to do the task list complete.</param>
    public CameraOperator(ICamera2DAdapter camera, ICameraOperatorTask idleTask)
    {
        _ownedCamera = camera;

        _taskList = new List<ICameraOperatorTask>();
        _idleTask = idleTask;
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

    private void HandleCameraStates(GameTime gameTime)
    {
        if (!_taskList.Any())
        {
            _idleTask.DoWork(gameTime, _ownedCamera);
            return;
        }

        var activeState = _taskList.First();
        activeState.DoWork(gameTime, _ownedCamera);

        if (activeState.IsComplete)
        {
            _taskList.Remove(activeState);

            if (!_taskList.Any())
            {
                _idleTask.DoWork(gameTime, _ownedCamera);
            }
        }
    }
}