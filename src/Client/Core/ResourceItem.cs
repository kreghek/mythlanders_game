namespace Client.Core;

internal sealed class ResourceItem
{
    public ResourceItem(EquipmentItemType type)
    {
        Type = type;
    }

    public int Amount { get; set; }
    public EquipmentItemType Type { get; }
}