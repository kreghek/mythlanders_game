using Client.Core;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

using JetBrains.Annotations;

namespace Client.Assets.MonsterPerks;

public interface IMonsterPerkFactory
{
    MonsterPerk Create();
}

public abstract class MonsterPerkFactoryBase : IMonsterPerkFactory
{
    protected string PerkName => GetType().Name[..^"MonsterPerkFactory".Length];

    protected abstract ICombatantStatusFactory CreateStatus();


    public MonsterPerk Create()
    {
        return new MonsterPerk(CreateStatus(), PerkName);
    }
}

[UsedImplicitly]
public sealed class ExtraHitPointsMonsterPerkFactory : MonsterPerkFactoryBase
{
    protected override ICombatantStatusFactory CreateStatus()
    {
        return new CombatStatusFactory(source =>
            new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(
                new CombatantStatusSid(nameof(PerkName)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                CombatantStatTypes.HitPoints,
                1)));
    }
}