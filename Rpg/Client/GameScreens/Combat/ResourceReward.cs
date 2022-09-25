using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat
{
    internal sealed record ResourceReward
    {
        public int Amount { get; init; }
        public int StartValue { get; init; }
        public EquipmentItemType Type { get; init; }
    }
}