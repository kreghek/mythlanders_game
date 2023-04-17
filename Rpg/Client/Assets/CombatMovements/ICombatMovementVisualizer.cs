using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementVisualizer
{
    CombatMovementIcon GetMoveIcon(CombatMovementSid sid);

    IActorVisualizationState GetMovementVisualizationState(CombatMovementSid sid, IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);
}