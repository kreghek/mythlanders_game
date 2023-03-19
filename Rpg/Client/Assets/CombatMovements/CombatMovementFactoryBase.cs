using Client.Engine;

using Core.Combats;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.CombatMovements;

internal abstract class CombatMovementFactoryBase : ICombatMovementFactory
{
    public virtual string Sid => GetType().Name[0..^7];
    public virtual CombatMovementIcon CombatMovementIcon => new(0, 0);

    public abstract CombatMovement CreateMovement();
    public virtual IActorVisualizationState CreateVisualization(IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext) => CommonCombatVisualization.CreateMeleeVisualization(actorAnimator, movementExecution,
            visualizationContext);
}
