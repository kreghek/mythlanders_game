using System.Collections.Generic;

using Client.Engine;
using Client.GameScreens;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatantStatuses;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Bogatyr;

[UsedImplicitly]
internal class RiseYourSwordsFactory : SimpleCombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(4, 2);

    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(2);
    }

    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(new IEffect[]
                {
                    new AddCombatantStatusEffect(
                        new AllAllyTargetSelector(),
                        new CombatStatusFactory(source =>
                        {
                            return new ImproveMeleeDamageCombatantStatus(new CombatantStatusSid(Sid),
                                new MultipleCombatantTurnEffectLifetime(1),
                                source,
                                1);
                        }))
                });
    }

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Bogatyr");

        var defenseAnimation = AnimationHelper.ConvertToAnimation(animationSet, "rise-swords");
        var defenseSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.Defence);

        return CommonCombatVisualization.CreateSelfBuffVisualization(actorAnimator, movementExecution,
            visualizationContext, defenseAnimation, defenseSoundEffect);
    }

    /// <inheritdoc />
    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("damage_buff", 1,
                DescriptionKeyValueTemplate.DamageModifier),
            new DescriptionKeyValue("duration", 1, DescriptionKeyValueTemplate.TurnDuration)
        };
    }
}