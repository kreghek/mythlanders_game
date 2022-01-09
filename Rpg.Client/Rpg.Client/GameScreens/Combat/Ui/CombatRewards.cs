using System.Collections.Generic;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed record CombatRewards
    {
        public ProgressionRewardStat BiomeProgress { get; init; }
        public IReadOnlyCollection<CombatRewardsItem> InventoryRewards { get; init; }
    }
}