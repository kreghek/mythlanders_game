using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;

namespace Client.Assets.MonsterPerks;

[UsedImplicitly]
public sealed class ExtraResolvePointsMonsterPerkFactory : MonsterPerkFactoryBase
{
    protected override Point IconCoords => IconHelper.GetMonsterPerkIconIndex(3, 0);

    protected override ICombatantStatusFactory CreateStatus()
    {
        return new CombatStatusFactory(source =>
            new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(
                new CombatantStatusSid(PerkName),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                CombatantStatTypes.Resolve,
                1)));
    }

    protected override IReadOnlyCollection<DescriptionKeyValue> CreateValues()
    {
        return new[]
        {
            new DescriptionKeyValue("resolve", 1, DescriptionKeyValueTemplate.Resolve)
        }.ToList();
    }
}