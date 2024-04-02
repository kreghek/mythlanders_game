using System.Collections.Generic;

using Client.Core;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using JetBrains.Annotations;

namespace Client.Assets.MonsterPerks;

[UsedImplicitly]
public sealed class FaithDefenderMonsterPerkFactory : MonsterPerkFactoryBase
{
    protected override bool IsUnique => true;

    protected override IReadOnlyCollection<IMonsterPerkPredicate> CreatePredicates()
    {
        return new[]
        {
            new OnlyForBlackConclaveMonsterPerkPredicate()
        };
    }

    protected override ICombatantStatusFactory CreateStatus()
    {
        return new CombatStatusFactory(source =>
            new ModifyEffectsCombatantStatus(new CombatantStatusSid(nameof(PerkName)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1));
    }
    protected override bool CantBeRolledAsReward => true;
}