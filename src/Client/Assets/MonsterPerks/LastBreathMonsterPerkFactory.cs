using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

using JetBrains.Annotations;

namespace Client.Assets.MonsterPerks;

[UsedImplicitly]
public sealed class LastBreathMonsterPerkFactory : MonsterPerkFactoryBase
{
    protected override ICombatantStatusFactory CreateStatus()
    {
        return new CombatStatusFactory(source =>
            new LastBreathCombatantStatus(new CombatantStatusSid(PerkName),
                new UntilCombatantEffectMeetPredicatesLifetime(new[]
                {
                    new OwnerStatBelowLifetimeExpirationCondition(CombatantStatTypes.HitPoints, 1)
                }),
                source,
                1,
                2));
    }
}