using Client.Engine;

using Core.Combats;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementFactory
{
    string Sid { get; }
    CombatMovement CreateMovement();
    IActorVisualizationState CreateVisualization(IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);
}