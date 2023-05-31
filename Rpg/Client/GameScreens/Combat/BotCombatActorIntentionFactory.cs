using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.States.Primitives;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;
using Core.Combats.BotBehaviour;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat;

namespace Client.GameScreens.Combat;

internal sealed class BotCombatActorIntentionFactory : IIntentionFactory
{
    private readonly IAnimationManager _animationManager;
    private readonly CameraOperator _cameraOperator;
    private readonly IList<CombatantGameObject> _combatantGameObjects;
    private readonly ICombatMovementVisualizationProvider _combatMovementVisualizer;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly InteractionDeliveryManager _interactionDeliveryManager;

    public BotCombatActorIntentionFactory(IAnimationManager animationManager,
        ICombatMovementVisualizationProvider combatMovementVisualizer,
        IList<CombatantGameObject> combatantGameObjects,
        InteractionDeliveryManager interactionDeliveryManager,
        GameObjectContentStorage gameObjectContentStorage,
        CameraOperator cameraOperator)
    {
        _animationManager = animationManager;
        _combatMovementVisualizer = combatMovementVisualizer;
        _combatantGameObjects = combatantGameObjects;
        _interactionDeliveryManager = interactionDeliveryManager;
        _gameObjectContentStorage = gameObjectContentStorage;
        _cameraOperator = cameraOperator;
    }

    public IIntention CreateCombatMovement(CombatMovementInstance combatMovement)
    {
        return new UseCombatMovementIntention(combatMovement, _animationManager, _combatMovementVisualizer,
            _combatantGameObjects, _interactionDeliveryManager, _gameObjectContentStorage, _cameraOperator);
    }
}