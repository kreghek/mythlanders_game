using System;

namespace Client.Core;

public sealed class OnlyForBlackConclaveMonsterPerkPredicate : IMonsterPerkPredicate
{
    public bool IsApplicableTo(MonsterCombatantPrefab monsterPrefab)
    {
        if (string.Equals(monsterPrefab.ClassSid, "aggressor", StringComparison.InvariantCultureIgnoreCase) ||
            string.Equals(monsterPrefab.ClassSid, "marauder", StringComparison.InvariantCultureIgnoreCase) ||
            string.Equals(monsterPrefab.ClassSid, "ambushdrone", StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        return false;
    }
}