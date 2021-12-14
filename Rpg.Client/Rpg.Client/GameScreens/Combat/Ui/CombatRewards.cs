using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed record CombatRewards
    {
        public IReadOnlyCollection<FoundEquipment> FoundEquipments { get; init; }
        public RewardStat BiomeProgress { get; init; }
        public IReadOnlyCollection<UnitRewards> UnitRewards { get; init; }
    }

    internal sealed record FoundEquipment(EquipmentItemType EquipmentItemType);
}