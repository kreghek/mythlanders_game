using Client.Core;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

namespace Client.Assets.MonsterPerks;

public static class MonsterPerkCatalog
{
    public static MonsterPerk ExtraHP { get; } = new(new CombatStatusFactory(source =>
            new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(
                new CombatantStatusSid(nameof(ExtraHP)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                CombatantStatTypes.HitPoints,
                1))),
        nameof(ExtraHP));

    public static MonsterPerk ExtraSP { get; } = new(new CombatStatusFactory(source =>
            new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(
                new CombatantStatusSid(nameof(ExtraSP)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                CombatantStatTypes.ShieldPoints,
                1))),
        nameof(ExtraSP));
        
    public static MonsterPerk ImprovedDamage { get; } = new(new CombatStatusFactory(source =>
            new ModifyEffectsCombatantStatus(new CombatantStatusSid(nameof(ImprovedDamage)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1)),
        nameof(ImprovedDamage));
}