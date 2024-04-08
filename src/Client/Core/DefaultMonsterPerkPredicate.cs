namespace Client.Core;

public sealed class DefaultMonsterPerkPredicate : IMonsterPerkPredicate
{
    public bool IsApplicableTo(MonsterCombatantPrefab monsterPrefab)
    {
        return true;
    }
}