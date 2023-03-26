using Client.Engine;

using Core.Combats;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementVisualizer
{
    CombatMovementIcon GetMoveIcon(CombatMovementSid sid);

    IActorVisualizationState GetMovementVisualizationState(CombatMovementSid sid, IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);
}