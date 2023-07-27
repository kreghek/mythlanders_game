using System.Collections.Generic;
using System.Linq;

using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Assets.CombatMovements;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects;
using Client.GameScreens.Combat.GameObjects.CommonStates;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;

namespace Client.GameScreens.Combat;

internal sealed class UseCombatMovementIntention : IIntention
{
    private readonly IAnimationManager _animationManager;
    private readonly CameraOperator _cameraOperator;
    private readonly IList<CombatantGameObject> _combatantGameObjects;
    private readonly CombatMovementInstance _combatMovement;
    private readonly ICombatMovementVisualizationProvider _combatMovementVisualizer;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly InteractionDeliveryManager _interactionDeliveryManager;
    private readonly IShadeService _shadeService;

    public UseCombatMovementIntention(CombatMovementInstance combatMovement, IAnimationManager animationManager,
        ICombatMovementVisualizationProvider combatMovementVisualizer, IList<CombatantGameObject> combatantGameObjects,
        InteractionDeliveryManager interactionDeliveryManager, GameObjectContentStorage gameObjectContentStorage,
        CameraOperator cameraOperator,
        IShadeService shadeService)
    {
        _combatMovement = combatMovement;
        _animationManager = animationManager;
        _combatMovementVisualizer = combatMovementVisualizer;
        _combatantGameObjects = combatantGameObjects;
        _interactionDeliveryManager = interactionDeliveryManager;
        _gameObjectContentStorage = gameObjectContentStorage;
        _cameraOperator = cameraOperator;
        _shadeService = shadeService;
    }

    private CombatantGameObject GetCombatantGameObject(ICombatant combatant)
    {
        return _combatantGameObjects.First(x => x.Combatant == combatant);
    }

    private CombatMovementScene GetMovementVisualizationState(CombatantGameObject actorGameObject,
        CombatMovementExecution movementExecution, CombatMovementInstance combatMovement)
    {
        var context = new CombatMovementVisualizationContext(
            actorGameObject,
            _combatantGameObjects.ToArray(),
            _interactionDeliveryManager,
            _gameObjectContentStorage,
            new BattlefieldInteractionContext(),
            new LinearDice());

        return _combatMovementVisualizer.GetMovementVisualizationState(combatMovement.SourceMovement.Sid,
            actorGameObject.Animator, movementExecution, context);
    }

    private void PlaybackCombatMovementExecution(CombatMovementExecution movementExecution,
        CombatMovementScene movementScene, CombatEngineBase combatCore)
    {
        var actorGameObject = GetCombatantGameObject(combatCore.CurrentCombatant);

        var mainAnimationBlocker = _animationManager.CreateAndRegisterBlocker();

        mainAnimationBlocker.Released += (_, _) =>
        {
            _shadeService.DropTargets();

            // Wait for some time to separate turns of different actors

            var delayBlocker = new DelayBlocker(new Duration(1));
            _animationManager.RegisterBlocker(delayBlocker);

            delayBlocker.Released += (_, _) =>
            {
                movementExecution.CompleteDelegate();
                combatCore.CompleteTurn();
            };
        };

        var actorState = new SequentialState(
            // Delay to focus on the current actor.
            new DelayActorState(new Duration(0.75f)),

            // Main move animation.
            movementScene.ActorState,

            // Release the main animation blocker to say the main move is ended.
            new AnimationBlockerTerminatorActorState(mainAnimationBlocker));

        actorGameObject.AddStateEngine(actorState);

        foreach (var cameraTask in movementScene.CameraOperatorTaskSequence)
        {
            _cameraOperator.AddState(cameraTask);
        }

        var targetCombatants = movementExecution.EffectImposeItems.SelectMany(x => x.MaterializedTargets).ToArray();
        var targetCombatantAnimators = targetCombatants.Select(x => GetCombatantGameObject(x).Animator).ToArray();
        var focusedAnimators = new HashSet<IActorAnimator>(targetCombatantAnimators)
        {
            actorGameObject.Animator
        };
        _shadeService.AddTargets(focusedAnimators);
    }

    public void Make(CombatEngineBase combatEngine)
    {
        var movementExecution = combatEngine.CreateCombatMovementExecution(_combatMovement);

        var actorGameObject = GetCombatantGameObject(combatEngine.CurrentCombatant);
        var movementState = GetMovementVisualizationState(actorGameObject, movementExecution, _combatMovement);

        PlaybackCombatMovementExecution(movementExecution, movementState, combatEngine);
    }
}