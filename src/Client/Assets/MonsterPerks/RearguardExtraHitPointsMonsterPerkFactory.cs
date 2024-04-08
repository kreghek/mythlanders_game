using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.AuraTargetSelectors;

using JetBrains.Annotations;

namespace Client.Assets.MonsterPerks;

[UsedImplicitly]
public sealed class RearguardExtraHitPointsMonsterPerkFactory : MonsterPerkFactoryBase
{
    protected override ICombatantStatusFactory CreateStatus()
    {
        return new CombatStatusFactory(source =>
            new AuraCombatantStatus(new CombatantStatusSid(PerkName),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                owner => new CombatStatusFactory(source2 =>
                    new ModifyStatCombatantStatus(
                        new CombatantStatusSid(PerkName),
                        new TargetCombatantsBoundCombatantStatusLifetime(owner),
                        source2,
                        CombatantStatTypes.HitPoints,
                        1)),
                new AllyRearguardAuraTargetSelector()
            ));
    }
    
    protected override IReadOnlyCollection<DescriptionKeyValue> CreateValues()
    {
        return new[]
        {
            new DescriptionKeyValue("hp", 1, DescriptionKeyValueTemplate.HitPoints)
        }.ToList();
    }
}