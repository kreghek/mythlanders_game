using Client.Assets.CombatMovements.Hero.Swordsman;
using Client.Engine;

using Core.Combats;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementVisualizer
{
    CombatMovementIcon GetMoveIcon(CombatMovementSid sid);

    CombatMovementScene GetMovementVisualizationState(CombatMovementSid sid, IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);
}