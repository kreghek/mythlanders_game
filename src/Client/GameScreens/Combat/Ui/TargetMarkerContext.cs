using System.Collections.Generic;
using System.Linq;

using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

namespace Client.GameScreens.Combat.Ui;

internal sealed class TargetMarkerContext : ITargetMarkerContext
{
    private readonly IReadOnlyCollection<CombatantGameObject> _combatantGameObjects;

    public TargetMarkerContext(CombatEngineBase combatCore, IReadOnlyCollection<CombatantGameObject> combatantGameObjects,
        TargetSelectorContext selectorContext)
    {
        _combatantGameObjects = combatantGameObjects;
        TargetSelectorContext = selectorContext;
        CurrentCombatant = combatCore.CurrentCombatant;
    }

    public Combatant CurrentCombatant { get; }

    public ITargetSelectorContext TargetSelectorContext { get; }

    public CombatantGameObject GetCombatantGameObject(Combatant combatant)
    {
        return _combatantGameObjects.First(x => x.Combatant == combatant);
    }
}