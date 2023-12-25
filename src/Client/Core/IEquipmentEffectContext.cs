namespace Client.Core;

internal interface IEquipmentEffectContext
{
    int EquipmentLevel { get; }
    bool IsInTankingSlot { get; }
}