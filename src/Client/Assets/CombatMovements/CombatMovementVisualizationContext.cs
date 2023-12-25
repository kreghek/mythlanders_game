using System.Collections.Generic;
using System.Linq;

using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Core;
using Client.Engine.PostProcessing;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;

using GameClient.Engine.CombatVisualEffects;

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
        ICombatVisualEffectManager visualEffectManager,
        IDice dice, PostEffectManager postEffectManager)
    {
        ActorGameObject = actorGameObject;
        _gameObjects = gameObjects;
        InteractionDeliveryManager = interactionDeliveryManager;
        GameObjectContentStorage = gameObjectContentStorage;
        BattlefieldInteractionContext = battlefieldInteractionContext;
        Dice = dice;
        PostEffectManager = postEffectManager;
        CombatVisualEffectManager = visualEffectManager;
    }

    public CombatantGameObject GetCombatActor(ICombatant combatant)
    {
        return _gameObjects.Single(x => x.Combatant == combatant);
    }

    public InteractionDeliveryManager InteractionDeliveryManager { get; }

    public GameObjectContentStorage GameObjectContentStorage { get; }

    public CombatantGameObject ActorGameObject { get; }

    public IBattlefieldInteractionContext BattlefieldInteractionContext { get; }

    public PostEffectManager PostEffectManager { get; }
    public IDice Dice { get; }
    public ICombatVisualEffectManager CombatVisualEffectManager { get; }
}