using Client.Assets.States.Primitives;
using Client.Core.AnimationFrameSets;
using System.Linq;

using Client.Engine;
using Client.GameScreens.Combat.GameObjects;
using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates;

using Rpg.Client.GameScreens.Combat.GameObjects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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

    public override IActorVisualizationState CreateVisualization(IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var config = new SingleMeleeVisualizationConfig(
                    new LinearAnimationFrameSet(Enumerable.Range(8, 8).ToArray(), 4, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8),
                    new LinearAnimationFrameSet(new[] { 0 }, 1, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8) { IsLoop = true });

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution, visualizationContext, config);
    }
}