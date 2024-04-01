using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.Catalogs;

internal interface IMonsterPerkManager
{
    IReadOnlyCollection<MonsterPerk> RollLocationPerks();
}