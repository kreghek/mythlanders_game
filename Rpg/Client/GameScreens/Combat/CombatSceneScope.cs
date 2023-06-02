using System.Collections.Generic;

using Client.Engine;

namespace Client.GameScreens.Combat;

/// <summary>
/// Base combat scope implementation.
/// </summary>
internal sealed class CombatSceneScope : ICombatShadeScope
{
    public IReadOnlyList<IActorAnimator> FocusedActors { get; }

    public CombatSceneScope(IReadOnlyList<IActorAnimator> actors)
    {
        FocusedActors = actors;
    }
}
