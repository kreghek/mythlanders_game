using System.Collections.Generic;

using Client.Core;

namespace Client.GameScreens.Combat;

internal record RewardCalculationContext(IEnumerable<ResourceItem> Inventory,
    EquipmentItemType? EquipmentResourceDrop, IEnumerable<CombatRewardInfo> CombatInfos,
    int BiomeCurrentLevel, int BiomeLevelToBoss);