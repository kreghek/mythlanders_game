﻿using System.Linq;

using Client.Core.AnimationFrameSets;
using Client.Engine;

using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using Rpg.Client.Core.AnimationFrameSets;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

internal class DieBySwordFactory : CombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(0, 0);

    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    ),
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(2))
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var keepSwordStrongerAnimation = new CompositeAnimationFrameSet(new[] {
            new LinearAnimationFrameSet(Enumerable.Range(8 * 2, 8).ToArray(), 8, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8),
            new LinearAnimationFrameSet(new[]{ 8 * 2 }, 1, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8)
        });

        var config = new SingleMeleeVisualizationConfig(
            keepSwordStrongerAnimation,
            new LinearAnimationFrameSet(Enumerable.Range(8, 8).ToArray(), 4, CommonConstants.FrameSize.X,
                CommonConstants.FrameSize.Y, 8),
            new LinearAnimationFrameSet(new[] { 0 }, 1, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8)
                { IsLoop = true });

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution,
            visualizationContext, config);
    }
}