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

using Steamworks;

namespace Client.Assets.CombatMovements.Hero.Hoplite;

[UsedImplicitly]
internal class InvinciblePhalanxFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(4, 3);

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
                new AddCombatantStatusEffect(new AllAllyColumnTargetSelector(),
                    new CombatStatusFactory(source =>
                        new DefensiveStanceCombatantStatusWrapper(
                            new AutoRestoreModifyStatCombatantStatus(
                                new ModifyStatCombatantStatus(
                                    new CombatantStatusSid(Sid),
                                    new ToNextCombatantTurnEffectLifetime(),
                                    source,
                                    CombatantStatTypes.Defense,
                                    1000))
                        )
                    )
                )
                {
                    ImposeConditions = new[] { new IsAllyColumnFilledShieldCondition() }
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