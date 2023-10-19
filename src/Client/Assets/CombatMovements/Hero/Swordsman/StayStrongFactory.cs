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

using JetBrains.Annotations;

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
}