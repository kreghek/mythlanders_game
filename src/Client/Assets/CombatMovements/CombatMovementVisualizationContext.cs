using System.Collections.Generic;
using System.Linq;

using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Core;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;
using Core.Dices;

namespace Client.Assets.CombatMovements;

internal sealed class CombatMovementVisualizationContext : ICombatMovementVisualizationContext
{
    private readonly IReadOnlyCollection<CombatantGameObject> _gameObjects;

    public CombatMovementVisualizationContext(
        CombatantGameObject actorGameObject,
        IReadOnlyCollection<CombatantGameObject> gameObjects,
        InteractionDeliveryManager interactionDeliveryManager,
        GameObjectContentStorage gameObjectContentStorage,
        IBattlefieldInteractionContext battlefieldInteractionContext,
        IDice dice)
    {
        ActorGameObject = actorGameObject;
        _gameObjects = gameObjects;
        InteractionDeliveryManager = interactionDeliveryManager;
        GameObjectContentStorage = gameObjectContentStorage;
        BattlefieldInteractionContext = battlefieldInteractionContext;
        Dice = dice;
    }

    public CombatantGameObject GetCombatActor(Combatant combatant)
    {
        return _gameObjects.Single(x => x.Combatant == combatant);
    }

    public InteractionDeliveryManager InteractionDeliveryManager { get; }

    public GameObjectContentStorage GameObjectContentStorage { get; }

    public CombatantGameObject ActorGameObject { get; }

    public IBattlefieldInteractionContext BattlefieldInteractionContext { get; }

    public IDice Dice { get; }
}