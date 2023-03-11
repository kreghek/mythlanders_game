using Client.Engine;

using Core.Combats;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementVisualizer
{
    IActorVisualizationState GetMovementVisualizationState(string sid, IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);
}
