using System.Collections.Generic;
using System.Linq;

using Client.Engine;

namespace Client.GameScreens.Combat;

internal sealed class CombatSceneContext : ICombatSceneContext
{
    public CombatSceneContext(IReadOnlyList<IActorAnimator> actors)
    {
        if (actors.Any())
        {
            CurrentScope = new CombatSceneScope(actors);
        }
    }

    public ICombatSceneScope? CurrentScope { get; }
}
