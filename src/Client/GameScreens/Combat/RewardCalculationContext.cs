using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat
{
    internal record RewardCalculationContext(IEnumerable<ResourceItem> Inventory,
        EquipmentItemType? EquipmentResourceDrop, IEnumerable<CombatRewardInfo> CombatInfos,
        int BiomeCurrentLevel, int BiomeLevelToBoss);
}