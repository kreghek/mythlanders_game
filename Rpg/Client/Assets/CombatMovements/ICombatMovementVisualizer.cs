using Client.Engine;

using Core.Combats;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementVisualizer
{
    IActorVisualizationState GetMovementVisualizationState(CombatMovementSid sid, IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);

    CombatMovementIcon GetMoveIcon(CombatMovementSid sid);
}