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

namespace Client.Assets.CombatMovements.Hero.Swordsman;

[UsedImplicitly]
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
                        new CombatStatusFactory(source =>
                        {
                            return new ImproveMeleeDamageCombatantStatus(new CombatantStatusSid(Sid),
                                new MultipleCombatantTurnEffectLifetime(1),
                                source,
                                1);
                        }))
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
    
    /// <inheritdoc />
    public override IReadOnlyList<CombatMovementEffectDisplayValue> ExtractEffectsValues(CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new CombatMovementEffectDisplayValue("damage_buff", 1, CombatMovementEffectDisplayValueTemplate.DamageModifier),
            new CombatMovementEffectDisplayValue("duration", 1, CombatMovementEffectDisplayValueTemplate.TurnDuration),
        };
    }
}