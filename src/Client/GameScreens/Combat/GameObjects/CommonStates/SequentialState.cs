using System.Collections.Generic;
using System.Linq;

using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects.CommonStates;

public sealed class SequentialState : IActorVisualizationState
{
    private readonly IReadOnlyList<IActorVisualizationState> _subStates;
    private int _subStateIndex;

    public SequentialState(params IActorVisualizationState[] subStates)
    {
        _subStates = subStates;
    }

    public bool CanBeReplaced => _subStates.All(x => x.CanBeReplaced);

    public bool IsComplete { get; private set; }

    public void Cancel()
    {
        // Nothing to release.
    }

    public void Update(GameTime gameTime)
    {
        if (IsComplete)
        {
            return;
        }

        if (_subStateIndex < _subStates.Count)
        {
            var currentSubState = _subStates[_subStateIndex];
            if (currentSubState.IsComplete)
            {
                _subStateIndex++;
            }
            else
            {
                currentSubState.Update(gameTime);
            }
        }
        else
        {
            IsComplete = true;
        }
    }
}