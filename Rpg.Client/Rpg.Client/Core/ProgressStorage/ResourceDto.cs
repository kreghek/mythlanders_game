namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record ResourceDto
    {
        public EquipmentItemType Type { get; init; }
        public int Amount { get; init; }
    }
}