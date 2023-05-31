using System.Linq;

using Client.Core.AnimationFrameSets;
using Client.Engine;

using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using Rpg.Client.Core;

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
        var swordsmanAnimationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Swordsman");

        var keepSwordStrongerAnimation = ConvertToAnimation(swordsmanAnimationSet, "prepare-sword");

        var chargeAnimation = ConvertToAnimation(swordsmanAnimationSet, "charge");

        var hitAnimation = ConvertToAnimation(swordsmanAnimationSet, "hit");

        var hitCompleteAnimation = ConvertToAnimation(swordsmanAnimationSet, "hit-complete");

        var backAnimation = ConvertToAnimation(swordsmanAnimationSet, "back");

        var config = new SingleMeleeVisualizationConfig(
            keepSwordStrongerAnimation,
            chargeAnimation,
            hitAnimation,
            hitCompleteAnimation,
            backAnimation);

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution,
            visualizationContext, config);
    }

    private static IAnimationFrameSet ConvertToAnimation(SpriteAtlasAnimationData spredsheetAnimationData, string animation)
    {
        var spredsheetAnimationDataCycles = spredsheetAnimationData.Cycles[animation];

        return new LinearAnimationFrameSet(
            spredsheetAnimationDataCycles.Frames,
            spredsheetAnimationDataCycles.Fps,
            spredsheetAnimationData.TextureAtlas.RegionWidth,
            spredsheetAnimationData.TextureAtlas.RegionHeight, 8)
        { 
            IsLooping = spredsheetAnimationDataCycles.IsLooping
        };
    }
}