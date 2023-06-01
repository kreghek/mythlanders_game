using System.Collections.Generic;

using Client.Engine;

namespace Client.GameScreens.Combat;

internal sealed class HighlightService : IHighlightService
{
    private readonly List<IActorAnimator> _currentActors = new List<IActorAnimator>();

    public void AddTargets(IReadOnlyCollection<IActorAnimator> animators)
    {
        _currentActors.AddRange(animators);
    }

    public void ClearTargets()
    {
        _currentActors.Clear();
    }

    public ICombatSceneContext CreateContext()
    {
        return new CombatSceneContext(_currentActors);
    }
}
