using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats.CombatantStatuses;

using JetBrains.Annotations;

namespace Client.Assets.MonsterPerks;

[UsedImplicitly]
public sealed class ImprovedMeleeDamageMonsterPerkFactory : MonsterPerkFactoryBase
{
    protected override ICombatantStatusFactory CreateStatus()
    {
        return new CombatStatusFactory(source =>
            new ImproveMeleeDamageCombatantStatus(new CombatantStatusSid(PerkName),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1));
    }

    protected override int IconIndex => IconHelper.GetMonsterPerkIconIndex(1, 1);
}