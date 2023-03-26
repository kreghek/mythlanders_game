using System.Collections.Generic;
using System.Linq;

using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

namespace Client.Assets.CombatMovements;

internal sealed class CombatMovementVisualizationContext : ICombatMovementVisualizationContext
{
    private readonly IReadOnlyCollection<CombatantGameObject> _gameObjects;

    public CombatMovementVisualizationContext(IReadOnlyCollection<CombatantGameObject> gameObjects)
    {
        _gameObjects = gameObjects;
    }

    public CombatantGameObject GetCombatActor(Combatant combatant)
    {
        return _gameObjects.Single(x => x.Combatant == combatant);
    }
}