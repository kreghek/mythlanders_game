using Client.Engine;
using Client.GameScreens;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using JetBrains.Annotations;

using SelfTargetSelector = Core.Combats.TargetSelectors.SelfTargetSelector;

namespace Client.Assets.CombatMovements.Hero.Robber;

[UsedImplicitly]
internal class WingsOfVelesFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(1, 7);

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Robber");

        var defenseAnimation = AnimationHelper.ConvertToAnimation(animationSet, "buff");
        var defenseSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.Defence);

        return CommonCombatVisualization.CreateSelfBuffVisualization(actorAnimator, movementExecution,
            visualizationContext, defenseAnimation, defenseSoundEffect);
    }

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(1);
    }

    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        var combatantEffectFactory = new ModifyCombatantMoveStatsCombatantStatusFactory(
            new CombatantStatusSid(Sid),
            new MultipleCombatantTurnEffectLifetimeFactory(1),
            CombatantMoveStats.Cost,
            -1);

        return CombatMovementEffectConfig.Create(new IEffect[]
        {
            new ModifyEffectsEffect(new CombatantStatusSid(Sid), new SelfTargetSelector(), 1),
            new AddCombatantStatusEffect(new SelfTargetSelector(), combatantEffectFactory)
        });
    }
}