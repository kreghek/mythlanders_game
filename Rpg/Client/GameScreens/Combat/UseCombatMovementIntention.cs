﻿using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.GameScreens.Combat;

internal sealed class UseCombatMovementIntention : IIntention
{
    private readonly CombatMovementInstance _combatMovement;
    private readonly IAnimationManager _animationManager;
    private readonly ICombatMovementVisualizer _combatMovementVisualizer;
    private readonly IList<CombatantGameObject> _combatantGameObjects;

    public UseCombatMovementIntention(CombatMovementInstance combatMovement, IAnimationManager animationManager, ICombatMovementVisualizer combatMovementVisualizer, IList<CombatantGameObject> combatantGameObjects)
    {
        _combatMovement = combatMovement;
        _animationManager = animationManager;
        _combatMovementVisualizer = combatMovementVisualizer;
        _combatantGameObjects = combatantGameObjects;
    }

    public void Make(CombatCore combatCore)
    {
        var movementExecution = combatCore.CreateCombatMovementExecution(_combatMovement);

        var actorGameObject = GetCombatantGameObject(combatCore.CurrentCombatant);
        var movementState = GetMovementVisualizationState(actorGameObject, movementExecution, _combatMovement);

        PlaybackCombatMovementExecution(movementState, combatCore);
    }

    private void PlaybackCombatMovementExecution(IActorVisualizationState movementState, CombatCore combatCore)
    {
        var actorGameObject = GetCombatantGameObject(combatCore.CurrentCombatant);

        var mainAnimationBlocker = _animationManager.CreateAndRegisterBlocker();

        mainAnimationBlocker.Released += (sender, args) =>
        {
            // Wait some time to separate turns of different actors

            var delayBlocker = new DelayBlocker(new Duration(1));
            _animationManager.RegisterBlocker(delayBlocker);

            delayBlocker.Released += (_, _) =>
            {
                combatCore.CompleteTurn();
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
    }

    private IActorVisualizationState GetMovementVisualizationState(CombatantGameObject actorGameObject, CombatMovementExecution movementExecution, CombatMovementInstance combatMovement)
    {
        var context = new CombatMovementVisualizationContext(_combatantGameObjects.ToArray());

        return _combatMovementVisualizer.GetMovementVisualizationState(combatMovement.SourceMovement.Sid, actorGameObject.Animator, movementExecution, context);
    }

    private CombatantGameObject GetCombatantGameObject(Combatant combatant)
    {
        return _combatantGameObjects.First(x => x.Combatant == combatant);
    }
}