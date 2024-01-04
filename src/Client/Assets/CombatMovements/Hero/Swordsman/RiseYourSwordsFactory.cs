using Client.Engine;
using Client.GameScreens;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

internal class RiseYourSwordsFactory : CombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(4, 2);

    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new AddCombatantStatusEffect(
                        new AllAllyTargetSelector(),
                        new ModifyEffectsCombatantStatusFactory(
                            new CombatantStatusSid(Sid),
                            new MultipleCombatantTurnEffectLifetimeFactory(1),
                            1))
                })
        );
    }

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var swordsmanAnimationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Swordsman");

        var defenseAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "rise-swords");
        var defenseSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.Defence);

        return CommonCombatVisualization.CreateSelfBuffVisualization(actorAnimator, movementExecution,
            visualizationContext, defenseAnimation, defenseSoundEffect);
    }
}