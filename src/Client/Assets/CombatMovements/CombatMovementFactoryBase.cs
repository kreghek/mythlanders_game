using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Core.AnimationFrameSets;
using Client.Engine;

using Core.Combats;

namespace Client.Assets.CombatMovements;

internal abstract class CombatMovementFactoryBase : ICombatMovementFactory
{
    private static IAnimationFrameSet CreateLinear(IReadOnlyList<int> frames, float fps)
    {
        return new LinearAnimationFrameSet(frames,
            fps,
            CommonConstants.FrameSize.X,
            CommonConstants.FrameSize.Y,
            CommonConstants.FrameCount);
    }

    private static IAnimationFrameSet CreateLoopingLinear(IReadOnlyList<int> frames, float fps)
    {
        return new LinearAnimationFrameSet(frames,
            fps,
            CommonConstants.FrameSize.X,
            CommonConstants.FrameSize.Y,
            CommonConstants.FrameCount)
        {
            IsLooping = true
        };
    }

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
            new SoundedAnimation(CreateLinear(new[] { 0 }, 1), null),
            new SoundedAnimation(CreateLinear(Enumerable.Range(0, 1).ToArray(), 8), null),
            new SoundedAnimation(CreateLinear(Enumerable.Range(0, 1).ToArray(), 8), null),
            new SoundedAnimation(CreateLinear(Enumerable.Range(0, 1).ToArray(), 8), null),
            new SoundedAnimation(CreateLoopingLinear(new[] { 0 }, 1), null));

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution,
            visualizationContext, config);
    }
}