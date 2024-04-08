using System.Collections.Generic;
using System.Linq;

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
    protected override int IconIndex => IconHelper.GetMonsterPerkIconIndex(2, 1);

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

    protected override IReadOnlyCollection<DescriptionKeyValue> CreateValues()
    {
        return new[]
        {
            new DescriptionKeyValue("hp", 1, DescriptionKeyValueTemplate.HitPoints),
            new DescriptionKeyValue("hp_restore", 2, DescriptionKeyValueTemplate.HitPoints)
        }.ToList();
    }
}