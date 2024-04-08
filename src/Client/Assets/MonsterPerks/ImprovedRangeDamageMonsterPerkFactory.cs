using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats.CombatantStatuses;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;

namespace Client.Assets.MonsterPerks;

[UsedImplicitly]
public sealed class ImprovedRangeDamageMonsterPerkFactory : MonsterPerkFactoryBase
{
    protected override Point IconCoords => IconHelper.GetMonsterPerkIconIndex(2, 0);

    protected override ICombatantStatusFactory CreateStatus()
    {
        return new CombatStatusFactory(source =>
            new ImproveRangeDamageCombatantStatus(new CombatantStatusSid(PerkName),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1));
    }

    protected override IReadOnlyCollection<DescriptionKeyValue> CreateValues()
    {
        return new[]
        {
            new DescriptionKeyValue("damage", 1, DescriptionKeyValueTemplate.DamageModifier)
        }.ToList();
    }
}