using System.Collections.Generic;
using System.Linq;

using Client.Engine;

namespace Client.GameScreens.Combat;

/// <summary>
/// Base imlementation of the combat scene shade state.
/// Create scope if any actors in the focus.
/// </summary>
internal sealed class CombatShadeContext : ICombatShadeContext
{
    public CombatShadeContext(IReadOnlyList<IActorAnimator> actors)
    {
        if (actors.Any())
        {
            CurrentScope = new CombatSceneScope(actors);
        }
    }

    /// <inheritdoc/>
    public ICombatShadeScope? CurrentScope { get; }
}
