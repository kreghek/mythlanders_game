using System.Collections.Generic;
using System.Linq;

using Client.Assets.States.Primitives;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

namespace Client.Assets.CombatMovements;

internal sealed class CombatMovementVisualizationContext : ICombatMovementVisualizationContext
{
    private readonly IReadOnlyCollection<CombatantGameObject> _gameObjects;

    public CombatMovementVisualizationContext(
        CombatantGameObject actorGameObject,
        IReadOnlyCollection<CombatantGameObject> gameObjects,
        InteractionDeliveryManager interactionDeliveryManager,
        GameObjectContentStorage gameObjectContentStorage)
    {
        ActorGameObject = actorGameObject;
        _gameObjects = gameObjects;
        InteractionDeliveryManager = interactionDeliveryManager;
        GameObjectContentStorage = gameObjectContentStorage;
    }

    public CombatantGameObject GetCombatActor(Combatant combatant)
    {
        return _gameObjects.Single(x => x.Combatant == combatant);
    }

    public InteractionDeliveryManager InteractionDeliveryManager { get; }

    public GameObjectContentStorage GameObjectContentStorage { get; }

    public CombatantGameObject ActorGameObject { get; }
}