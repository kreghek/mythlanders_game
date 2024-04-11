using System.Collections.Generic;
using System.Linq;

using Client.Core;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;
using GameAssets.Combats.CombatMovementEffects;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Hoplite;

[UsedImplicitly]
internal class PhalanxFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(4, 4); //IconOneBasedIndex = 23

    /// <inheritdoc />
    protected override IEnumerable<CombatMovementMetadataTrait> CreateTraits()
    {
        yield return CombatMovementMetadataTraits.Melee;
    }

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(2);
    }

    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return new CombatMovementEffectConfig(new IEffect[]
            {
                new AddCombatantStatusEffect(new SelfTargetSelector(),
                    new CombatStatusFactory(source =>
                        new DefensiveStanceCombatantStatusWrapper(
                            new AutoRestoreModifyStatCombatantStatus(
                                new ModifyStatCombatantStatus(
                                    new CombatantStatusSid(Sid),
                                    new ToNextCombatantTurnEffectLifetime(),
                                    source,
                                    CombatantStatTypes.Defense,
                                    1)))
                    )
                ),
                new AddCombatantStatusEffect(new LeftAllyTargetSelector(),
                    new CombatStatusFactory(source =>
                        new DefensiveStanceCombatantStatusWrapper(
                            new AutoRestoreModifyStatCombatantStatus(
                                new ModifyStatCombatantStatus(
                                    new CombatantStatusSid(Sid),
                                    new ToNextCombatantTurnEffectLifetime(),
                                    source,
                                    CombatantStatTypes.Defense,
                                    2))
                        )
                    )
                )
                {
                    ImposeConditions = new[] { new IsRightAllyWithShieldCondition() }
                }
            },
            new IEffect[]
            {
                new AddCombatantStatusEffect(new SelfTargetSelector(),
                    new CombatStatusFactory(source =>
                        new DefensiveStanceCombatantStatusWrapper(
                            new AutoRestoreModifyStatCombatantStatus(
                                new ModifyStatCombatantStatus(
                                    new CombatantStatusSid(Sid),
                                    new ToEndOfCurrentRoundEffectLifetime(),
                                    source,
                                    CombatantStatTypes.Defense,
                                    1))
                        )
                    )
                )
            });
    }

    /// <inheritdoc />
    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }
}

//public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateProtection(SkillSid.DefenseStance, SkillDirection.Self, 1, 0.75f),
//            SkillRuleFactory.CreateProtection(SkillSid.DefenseStance, new LeftAllyTargetSelector(), duration: 1,
//                multiplier: 0.75f),
//            SkillRuleFactory.CreateProtection(SkillSid.DefenseStance, SkillDirection.Self, duration: 1,
//                multiplier: 0.25f, imposeConditions: new[] { new IsRightAllyWithShieldCondition() })
//        };
//        private static int? GetRightIndex(int baseIndex)
//        {
//            return baseIndex switch
//            {
//                0 => 2,
//                1 => 0,
//                3 => 5,
//                4 => 3,
//                _ => null
//            };
//        }

//        private sealed class IsRightAllyWithShieldCondition : IEffectCondition
//        {
//            public bool Check()
//            {
//                //var aliveAllies = effectContext.Combat.AliveUnits.Where(x => x.Unit.IsPlayerControlled);

//                //var selfIndex = ((CombatUnit)target).SlotIndex;

//                //var targetIndex = GetRightIndex(selfIndex);

//                //if (targetIndex is null)
//                //{
//                //    return false;
//                //}

//                //var rightAllyUnit = aliveAllies.SingleOrDefault(x => ((CombatUnit)x).SlotIndex == targetIndex);

//                //if (rightAllyUnit is not null)
//                //{
//                //    return rightAllyUnit.Unit.UnitScheme.Name == UnitName.Assaulter;
//                //}

//                return false;
//            }

//            public string GetDescription()
//            {
//                return UiResource.EffectConditionIsRightAllyWithShieldText;
//            }
//        }