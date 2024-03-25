using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats.CombatantStatuses;

using JetBrains.Annotations;

namespace Client.Assets.MonsterPerks;

[UsedImplicitly]
public sealed class ReduceEnemyShieldPointsMonsterPerkFactory : MonsterPerkFactoryBase
{
    protected override ICombatantStatusFactory CreateStatus()
    {
        return new CombatStatusFactory(source =>
            new ReduceEnemiesShieldPointsCombatantStatus(
                new CombatantStatusSid(nameof(PerkName)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1));
    }
}