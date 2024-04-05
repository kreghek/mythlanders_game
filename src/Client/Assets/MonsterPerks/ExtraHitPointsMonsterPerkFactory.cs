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
public sealed class ExtraHitPointsMonsterPerkFactory : MonsterPerkFactoryBase
{
    protected override int IconIndex => IconHelper.GetMonsterPerkIconIndex(1, 0);

    protected override ICombatantStatusFactory CreateStatus()
    {
        return new CombatStatusFactory(source =>
            new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(
                new CombatantStatusSid(PerkName),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                CombatantStatTypes.HitPoints,
                1)));
    }
    
    protected override IReadOnlyCollection<DescriptionKeyValue> CreateValues()
    {
        return new[]
        {
            new DescriptionKeyValue("hp", 1, DescriptionKeyValueTemplate.HitPoints)
        }.ToList();
    }
}