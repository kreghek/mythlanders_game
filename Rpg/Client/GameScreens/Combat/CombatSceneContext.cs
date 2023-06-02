using System.Collections.Generic;
using System.Linq;

using Client.Engine;

namespace Client.GameScreens.Combat;

/// <summary>
/// Base imlementation of the combat scene context.
/// </summary>
internal sealed class CombatSceneContext : ICombatSceneContext
{
    public CombatSceneContext(IReadOnlyList<IActorAnimator> actors)
    {
        if (actors.Any())
        {
            CurrentScope = new CombatSceneScope(actors);
        }
    }

    /// <inheritdoc/>
    public ICombatSceneScope? CurrentScope { get; }
}
