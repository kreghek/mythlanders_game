using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects.CommonStates;

internal sealed class ParallelState : IActorVisualizationState
{
    private readonly IReadOnlyList<IActorVisualizationState> _subStates;

    public ParallelState(IReadOnlyList<IActorVisualizationState> subStates)
    {
        _subStates = subStates;
    }

    public bool CanBeReplaced => true;

    public bool IsComplete => _subStates.All(x => x.IsComplete);

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

        foreach (var subState in _subStates)
        {
            subState.Update(gameTime);
        }
    }
}