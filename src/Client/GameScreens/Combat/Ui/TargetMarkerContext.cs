using System.Collections.Generic;
using System.Linq;

using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using GameAssets.Combats;

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

    public ICombatant CurrentCombatant { get; }

    public ITargetSelectorContext TargetSelectorContext { get; }

    public CombatantGameObject GetCombatantGameObject(ICombatant combatant)
    {
        return _combatantGameObjects.First(x => x.Combatant == combatant);
    }
}