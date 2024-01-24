using Client.Core;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

namespace Client.Assets.MonsterPerks;

public static class MonsterPerkCatalog
{
    public static MonsterPerk ExtraHp { get; } = new(new CombatStatusFactory(source =>
            new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(
                new CombatantStatusSid(nameof(ExtraHp)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                CombatantStatTypes.HitPoints,
                1))),
        nameof(ExtraHp));

    public static MonsterPerk ExtraSp { get; } = new(new CombatStatusFactory(source =>
            new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(
                new CombatantStatusSid(nameof(ExtraSp)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                CombatantStatTypes.ShieldPoints,
                1))),
        nameof(ExtraSp));
        
    public static MonsterPerk ImprovedAllDamage { get; } = new(new CombatStatusFactory(source =>
            new ModifyEffectsCombatantStatus(new CombatantStatusSid(nameof(ImprovedAllDamage)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1)),
        nameof(ImprovedAllDamage));
    
    public static MonsterPerk ImprovedMeleeDamage { get; } = new(new CombatStatusFactory(source =>
            new ImproveMeleeDamageCombatantStatus(new CombatantStatusSid(nameof(ImprovedMeleeDamage)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1)),
        nameof(ImprovedMeleeDamage));
}