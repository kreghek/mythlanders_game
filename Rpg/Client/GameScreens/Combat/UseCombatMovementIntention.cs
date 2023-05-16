using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Assets.States.Primitives;
using Client.GameScreens.Combat;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates;

namespace Rpg.Client.GameScreens.Combat;

internal sealed class UseCombatMovementIntention : IIntention
{
    private readonly IAnimationManager _animationManager;
    private readonly CameraOperator _cameraOperator;
    private readonly IList<CombatantGameObject> _combatantGameObjects;
    private readonly CombatMovementInstance _combatMovement;
    private readonly ICombatMovementVisualizer _combatMovementVisualizer;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly InteractionDeliveryManager _interactionDeliveryManager;

    public UseCombatMovementIntention(CombatMovementInstance combatMovement, IAnimationManager animationManager,
        ICombatMovementVisualizer combatMovementVisualizer, IList<CombatantGameObject> combatantGameObjects,
        InteractionDeliveryManager interactionDeliveryManager, GameObjectContentStorage gameObjectContentStorage,
        CameraOperator cameraOperator)
    {
        _combatMovement = combatMovement;
        _animationManager = animationManager;
        _combatMovementVisualizer = combatMovementVisualizer;
        _combatantGameObjects = combatantGameObjects;
        _interactionDeliveryManager = interactionDeliveryManager;
        _gameObjectContentStorage = gameObjectContentStorage;
        _cameraOperator = cameraOperator;
    }

    private CombatantGameObject GetCombatantGameObject(Combatant combatant)
    {
        return _combatantGameObjects.First(x => x.Combatant == combatant);
    }

    private IActorVisualizationState GetMovementVisualizationState(CombatantGameObject actorGameObject,
        CombatMovementExecution movementExecution, CombatMovementInstance combatMovement)
    {
        var context = new CombatMovementVisualizationContext(actorGameObject, _combatantGameObjects.ToArray(),
            _interactionDeliveryManager, _gameObjectContentStorage);

        return _combatMovementVisualizer.GetMovementVisualizationState(combatMovement.SourceMovement.Sid,
            actorGameObject.Animator, movementExecution, context);
    }

    private void PlaybackCombatMovementExecution(CombatMovementExecution movementExecution,
        IActorVisualizationState movementState, CombatCore combatCore)
    {
        var actorGameObject = GetCombatantGameObject(combatCore.CurrentCombatant);

        var mainAnimationBlocker = _animationManager.CreateAndRegisterBlocker();

        var executionIsComplete = false;

        mainAnimationBlocker.Released += (_, _) =>
        {
            // Wait some time to separate turns of different actors

            var delayBlocker = new DelayBlocker(new Duration(1));
            _animationManager.RegisterBlocker(delayBlocker);

            delayBlocker.Released += (_, _) =>
            {
                movementExecution.CompleteDelegate();
                combatCore.CompleteTurn();

                executionIsComplete = true;
            };
        };

        var actorState = new SequentialState(
            // Delay to focus on the current actor.
            new DelayActorState(new Duration(0.75f)),

            // Main move animation.
            movementState,

            // Release the main animation blocker to say the main move is ended.
            new AnimationBlockerTerminatorActorState(mainAnimationBlocker));

        actorGameObject.AddStateEngine(actorState);

        _cameraOperator.AddState(
            new FollowActorOperatorCameraTask(actorGameObject.Animator, () => executionIsComplete));
    }

    public void Make(CombatCore combatCore)
    {
        var movementExecution = combatCore.CreateCombatMovementExecution(_combatMovement);

        var actorGameObject = GetCombatantGameObject(combatCore.CurrentCombatant);
        var movementState = GetMovementVisualizationState(actorGameObject, movementExecution, _combatMovement);

        PlaybackCombatMovementExecution(movementExecution, movementState, combatCore);
    }
}