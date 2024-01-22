using System.Collections.Generic;

using Client.Assets.MonsterPerks;
using Client.Core;

namespace Client.Assets;

internal static class PerkHelper
{
    public static IReadOnlyCollection<MonsterPerk> GetAllMonsterPerks()
    {
        return CatalogHelper.GetAllFromStaticCatalog<MonsterPerk>(typeof(MonsterPerkCatalog));
    }
}