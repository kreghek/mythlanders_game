﻿using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects.CommonStates;
using Client.GameScreens.Combat.GameObjects;
using Client.GameScreens.Combat;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.CombatantStatuses;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;
using GameAssets.Combats.CombatMovementEffects;
using GameAssets.Combats.TargetSelectors;

using GameClient.Engine.MoveFunctions;

using JetBrains.Annotations;
using Client.Core.AnimationFrameSets;
using Client.GameScreens;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

[UsedImplicitly]
internal class StayStrongFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(2, 0);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            new CombatMovementEffectConfig(
                new IEffect[]
                {
                    new AddCombatantStatusEffect(new SelfTargetSelector(),
                        new DelegateCombatStatusFactory(() =>
                            new AutoRestoreChangeStatCombatantStatus(
                                new ChangeStatCombatantStatus(
                                    new CombatantEffectSid(Sid),
                                    new ToNextCombatantTurnEffectLifetime(),
                                    CombatantStatTypes.Defense,
                                    3)))
                            ),
                    new DamageEffectWrapper(new AttackerTargetSelector(), DamageType.Normal,
                        GenericRange<int>.CreateMono(2))
                },
                new IEffect[]
                {
                    new AddCombatantStatusEffect(new SelfTargetSelector(),
                        new DelegateCombatStatusFactory(() =>
                            new AutoRestoreChangeStatCombatantStatus(
                                new ChangeStatCombatantStatus(
                                    new CombatantEffectSid(Sid),
                                    new ToEndOfCurrentRoundEffectLifetime(),
                                    CombatantStatTypes.Defense,
                                    1)))
                            )
                })
        )
        {
            Tags = CombatMovementTags.AutoDefense
        };
    }

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var swordsmanAnimationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Swordsman");

        var defenceAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "defense");
        var defenceSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.Defence);

        return CommonCombatVisualization.CreateSelfBuffVisualization(actorAnimator, movementExecution,
            visualizationContext, defenceAnimation, defenceSoundEffect);
    }
}