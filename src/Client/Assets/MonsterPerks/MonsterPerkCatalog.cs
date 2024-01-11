using Client.Core;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using Core.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

namespace Client.Assets.MonsterPerks;

public static class MonsterPerkCatalog
{
    public static MonsterPerk ExtraHP { get; } = new(new DelegateCombatStatusFactory(() =>
            new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(
                new CombatantStatusSid(nameof(ExtraHP)),
                new OwnerBoundCombatantEffectLifetime(), CombatantStatTypes.HitPoints, 1))),
        nameof(ExtraHP));

    public static MonsterPerk ExtraSP { get; } = new(new DelegateCombatStatusFactory(() =>
            new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(
                new CombatantStatusSid(nameof(ExtraSP)),
                new OwnerBoundCombatantEffectLifetime(), CombatantStatTypes.ShieldPoints, 1))),
        nameof(ExtraSP));
}