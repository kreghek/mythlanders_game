using System.Linq;

using Client.Assets.CombatMovements.Hero.Swordsman;
using Client.Core.AnimationFrameSets;
using Client.Engine;

using Core.Combats;

namespace Client.Assets.CombatMovements;

internal abstract class CombatMovementFactoryBase : ICombatMovementFactory
{
    /// <summary>
    /// Symbolic identifier of the combat movement.
    /// </summary>
    public virtual string Sid => GetType().Name[..^7];

    /// <summary>
    /// UI icon of the combat movement.
    /// </summary>
    public virtual CombatMovementIcon CombatMovementIcon => new(0, 0);

    /// <summary>
    /// Created combat movement definition.
    /// </summary>
    /// <returns>Combat movement definition</returns>
    public abstract CombatMovement CreateMovement();

    /// <summary>
    /// Creates actor's visualization state to animate combat move execution.
    /// </summary>
    /// <param name="actorAnimator">Animator to visualize combat move execution.</param>
    /// <param name="movementExecution">Materialized move execution.</param>
    /// <param name="visualizationContext">Combat context to interact with combat field.</param>
    /// <returns></returns>
    public virtual CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution,
        ICombatMovementVisualizationContext visualizationContext)
    {
        var config = new SingleMeleeVisualizationConfig(
            new LinearAnimationFrameSet(Enumerable.Range(0, 1).ToArray(), 8, CommonConstants.FrameSize.X,
                CommonConstants.FrameSize.Y, 8),
            new LinearAnimationFrameSet(new[] { 0 }, 1, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8)
                { IsLoop = true });

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution, visualizationContext, config);
    }
}