using System.Collections.Generic;

using Client.Engine;

namespace Client.GameScreens.Combat;

internal sealed class CombatSceneScope : ICombatSceneScope
{
    public IReadOnlyList<IActorAnimator> Actors { get; }

    public CombatSceneScope(IReadOnlyList<IActorAnimator> actors)
    {
        Actors = actors;
    }
}
