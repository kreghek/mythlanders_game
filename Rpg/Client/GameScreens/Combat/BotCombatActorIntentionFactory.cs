using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.States.Primitives;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;
using Core.Combats.BotBehaviour;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Client.GameScreens.Combat;

internal sealed class BotCombatActorIntentionFactory : IIntentionFactory
{
    private readonly IAnimationManager _animationManager;
    private readonly IList<CombatantGameObject> _combatantGameObjects;
    private readonly InteractionDeliveryManager _interactionDeliveryManager;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly ICombatMovementVisualizer _combatMovementVisualizer;

    public BotCombatActorIntentionFactory(IAnimationManager animationManager,
        ICombatMovementVisualizer combatMovementVisualizer,
        IList<CombatantGameObject> combatantGameObjects,
        InteractionDeliveryManager interactionDeliveryManager,
        GameObjectContentStorage gameObjectContentStorage)
    {
        _animationManager = animationManager;
        _combatMovementVisualizer = combatMovementVisualizer;
        _combatantGameObjects = combatantGameObjects;
        _interactionDeliveryManager = interactionDeliveryManager;
        _gameObjectContentStorage = gameObjectContentStorage;
    }

    public IIntention CreateCombatMovement(CombatMovementInstance combatMovement)
    {
        return new UseCombatMovementIntention(combatMovement, _animationManager, _combatMovementVisualizer,
            _combatantGameObjects, _interactionDeliveryManager, _gameObjectContentStorage);
    }
}