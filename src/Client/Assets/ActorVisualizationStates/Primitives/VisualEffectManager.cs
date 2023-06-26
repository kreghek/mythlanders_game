using System.Collections.Generic;
using System.Linq;

using Client.Assets.States.Primitives;

namespace Client.Assets.ActorVisualizationStates.Primitives;

internal sealed class VisualEffectManager
{
    private readonly IList<ICombatVisualEffect> _list = new List<ICombatVisualEffect>();

    public IReadOnlyCollection<ICombatVisualEffect> Effects => _list.ToArray();

    public void AddEffect(ICombatVisualEffect combatVisualEffect)
    {
        _list.Add(combatVisualEffect);
    }

    public void RemoveEffect(ICombatVisualEffect combatVisualEffect)
    {
        _list.Add(combatVisualEffect);
    }
}