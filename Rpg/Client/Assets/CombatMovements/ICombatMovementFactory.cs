﻿using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementFactory
{
    CombatMovementIcon CombatMovementIcon { get; }
    string Sid { get; }
    CombatMovement CreateMovement();

    IActorVisualizationState CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);
}