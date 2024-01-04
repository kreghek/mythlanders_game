using Client.Engine;
using Client.GameScreens;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Robber;

internal class WingsOfVelesFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(1, 7);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        var combatantEffectFactory = new ModifyCombatantMoveStatsCombatantStatusFactory(
            new CombatantStatusSid(Sid),
            new MultipleCombatantTurnEffectLifetimeFactory(1),
            CombatantMoveStats.Cost,
            -1);

        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new ModifyEffectsEffect(new CombatantStatusSid(Sid), new SelfTargetSelector(), 1),
                    new AddCombatantStatusEffect(new SelfTargetSelector(), combatantEffectFactory)
                })
        );
    }

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var swordsmanAnimationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Robber");

        var defenseAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "buff");
        var defenseSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.Defence);

        return CommonCombatVisualization.CreateSelfBuffVisualization(actorAnimator, movementExecution,
            visualizationContext, defenseAnimation, defenseSoundEffect);
    }
}