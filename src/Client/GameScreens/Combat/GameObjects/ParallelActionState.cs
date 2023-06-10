using System.Linq;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects;

public sealed class ParallelActionState : IActorVisualizationState
{
    private readonly IActorVisualizationState[] _states;

    public ParallelActionState(params IActorVisualizationState[] states)
    {
        _states = states;
    }

    public bool CanBeReplaced => _states.Any(x => x.CanBeReplaced);

    public bool IsComplete => _states.All(x => x.IsComplete);

    public void Cancel()
    {
        foreach (var state in _states)
        {
            state.Cancel();
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (var state in _states)
        {
            state.Update(gameTime);
        }
    }
}