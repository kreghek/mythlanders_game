using Client.Engine;

using Core.Combats;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementFactory
{
    CombatMovementIcon CombatMovementIcon { get; }
    string Sid { get; }
    CombatMovement CreateMovement();

    CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);
}