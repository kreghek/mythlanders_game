using Client.Engine;

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