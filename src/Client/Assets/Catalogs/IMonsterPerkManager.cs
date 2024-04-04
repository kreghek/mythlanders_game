using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.Catalogs;

internal interface IMonsterPerkManager
{
    IReadOnlyCollection<MonsterPerk> RollLocationRewardPerks();
    IReadOnlyCollection<MonsterPerk> RollMonsterPerks(MonsterCombatantPrefab targetMonsterPrefab);
}