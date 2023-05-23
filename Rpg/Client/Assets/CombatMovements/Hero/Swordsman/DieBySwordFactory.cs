using System.Linq;

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
            new LinearAnimationFrameSet(Enumerable.Range(8 * 3 + 5, 1).ToArray(), 1, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8)
        });

        var chargeAnimation = new LinearAnimationFrameSet(Enumerable.Range(8 + 4, 2).ToArray(), 4, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8);

        var hitAnimation = new LinearAnimationFrameSet(Enumerable.Range(8 + 2, 8 -2).ToArray(), 4, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8);

        var config = new SingleMeleeVisualizationConfig(
            keepSwordStrongerAnimation,
            chargeAnimation,
            hitAnimation,
            new LinearAnimationFrameSet(new[] { 0 }, 1, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8)
                { IsLoop = true });

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution,
            visualizationContext, config);
    }
}