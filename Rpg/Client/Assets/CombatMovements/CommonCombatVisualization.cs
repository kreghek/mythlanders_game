using System.Collections.Generic;
using System.Linq;

using Client.Assets.States.Primitives;
using Client.Core.AnimationFrameSets;
using Client.Engine;

using Core.Combats;

using Microsoft.Xna.Framework;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.CombatMovements;
internal static class CommonCombatVisualization
{
    public static IActorVisualizationState CreateMeleeVisualization(IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var skillAnimationInfo = new SkillAnimationInfo
        {
            Items = new[]
            {
                new SkillAnimationInfoItem
                {
                    Duration = 0.75f,
                    //HitSound = hitSound,
                    Interaction = () =>
                        Interaction(movementExecution.EffectImposeItems),
                    InteractTime = 0
                }
            }
        };

        var startPosition = actorAnimator.GraphicRoot.Position;
        var targetCombatUnit = GetFirstTargetOrDefault(movementExecution);

        Vector2 targetPosition;

        if (targetCombatUnit is not null)
        {
            var targetActor = visualizationContext.GetCombatActor(targetCombatUnit);

            targetPosition = targetActor.Graphics.Root.Position;
        }
        else
        {
            targetPosition = actorAnimator.GraphicRoot.Position;
        }

        var subStates = new IActorVisualizationState[]
        {
            new MoveToPositionActorState(actorAnimator,
                new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position, targetPosition),
                new LinearAnimationFrameSet(Enumerable.Range(8, 2).ToArray(), 8, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8)),
            new DirectInteractionState(actorAnimator, skillAnimationInfo,
                new LinearAnimationFrameSet(Enumerable.Range(10, 8).ToArray(), 8, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8)),
            new MoveToPositionActorState(actorAnimator,
                new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position, startPosition),
                new LinearAnimationFrameSet(new[] { 0 }, 1, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8)
                    { IsLoop = true })
        };

        var _innerState = new SequentialState(subStates);
        return _innerState;
    }

    private static Combatant? GetFirstTargetOrDefault(CombatMovementExecution movementExecution)
    {
        var firstImposeItem = movementExecution.EffectImposeItems.First();

        var targetCombatUnit = firstImposeItem.MaterializedTargets.FirstOrDefault();
        return targetCombatUnit;
    }

    private static void Interaction(IReadOnlyCollection<CombatEffectImposeItem> effectImposeItems)
    {
        foreach (var effectImposeItem in effectImposeItems)
        {
            foreach (var target in effectImposeItem.MaterializedTargets)
            {
                effectImposeItem.ImposeDelegate(target);
            }
        }
    }
}