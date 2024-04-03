namespace Client.Core;

public interface IMonsterPerkPredicate
{
    bool IsApplicableTo(MonsterCombatantPrefab monsterPrefab);
}