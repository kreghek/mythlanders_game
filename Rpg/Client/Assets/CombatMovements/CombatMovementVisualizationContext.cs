using System.Collections.Generic;
using System.Linq;

using Client.Assets.States.Primitives;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Rpg.Client.GameScreens;

namespace Client.Assets.CombatMovements;

internal sealed class CombatMovementVisualizationContext : ICombatMovementVisualizationContext
{
    private readonly IReadOnlyCollection<CombatantGameObject> _gameObjects;
    private readonly InteractionDeliveryManager _interactionDeliveryManager;
    private readonly GameObjectContentStorage _gameObjectContentStorage;

    public CombatMovementVisualizationContext(
        CombatantGameObject actorGameObject,
        IReadOnlyCollection<CombatantGameObject> gameObjects,
        InteractionDeliveryManager interactionDeliveryManager,
        GameObjectContentStorage gameObjectContentStorage)
    {
        ActorGameObject = actorGameObject;
        _gameObjects = gameObjects;
        _interactionDeliveryManager = interactionDeliveryManager;
        _gameObjectContentStorage = gameObjectContentStorage;
    }

    public CombatantGameObject GetCombatActor(Combatant combatant)
    {
        return _gameObjects.Single(x => x.Combatant == combatant);
    }

    public InteractionDeliveryManager InteractionDeliveryManager => _interactionDeliveryManager;
    public GameObjectContentStorage GameObjectContentStorage => _gameObjectContentStorage;

    public CombatantGameObject ActorGameObject { get; }
}