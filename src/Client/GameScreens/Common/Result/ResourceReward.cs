using Client.Core;

namespace Client.GameScreens.Common.Result;

internal sealed record ResourceReward
{
    public int Amount { get; init; }
    public int StartValue { get; init; }
    public EquipmentItemType Type { get; init; }
}