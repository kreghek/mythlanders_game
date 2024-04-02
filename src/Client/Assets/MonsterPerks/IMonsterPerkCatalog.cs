using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.MonsterPerks;

public interface IMonsterPerkCatalog
{
    IReadOnlyCollection<MonsterPerk> Perks { get; }
}