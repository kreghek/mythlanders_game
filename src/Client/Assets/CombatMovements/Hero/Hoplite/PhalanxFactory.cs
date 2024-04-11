using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;
using GameAssets.Combats.EffectConditions;

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
}