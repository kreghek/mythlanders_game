using System;

using Client.Assets.CombatMovements;
using Client.Core;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

namespace Client.Assets.MonsterPerks;

internal static class MonsterPerkCatalog
{
    public static MonsterPerk ExtraHp { get; } = new(new CombatStatusFactory(source =>
            new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(
                new CombatantStatusSid(nameof(ExtraHp)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                CombatantStatTypes.HitPoints,
                1))),
        nameof(ExtraHp),
        new[]
        {
            new CombatMovementEffectDisplayValue("hp", 1, CombatMovementEffectDisplayValueTemplate.HitPoints)
        });

    public static MonsterPerk ExtraSp { get; } = new(new CombatStatusFactory(source =>
            new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(
                new CombatantStatusSid(nameof(ExtraSp)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                CombatantStatTypes.ShieldPoints,
                1))),
        nameof(ExtraSp),
        new[]
        {
            new CombatMovementEffectDisplayValue("hp", 1, CombatMovementEffectDisplayValueTemplate.ShieldPoints)
        });

    public static MonsterPerk ImprovedAllDamage { get; } = new(new CombatStatusFactory(source =>
            new ModifyEffectsCombatantStatus(new CombatantStatusSid(nameof(ImprovedAllDamage)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1)),
        nameof(ImprovedAllDamage),
        Array.Empty<CombatMovementEffectDisplayValue>());

    public static MonsterPerk ImprovedMeleeDamage { get; } = new(new CombatStatusFactory(source =>
            new ImproveMeleeDamageCombatantStatus(new CombatantStatusSid(nameof(ImprovedMeleeDamage)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1)),
        nameof(ImprovedMeleeDamage), Array.Empty<CombatMovementEffectDisplayValue>());

    public static MonsterPerk ImprovedRangeDamage { get; } = new(new CombatStatusFactory(source =>
            new ImproveRangeDamageCombatantStatus(new CombatantStatusSid(nameof(ImprovedRangeDamage)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1)),
        nameof(ImprovedRangeDamage), Array.Empty<CombatMovementEffectDisplayValue>());
}