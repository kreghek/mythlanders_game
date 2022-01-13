namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record EquipmentDto
    {
        public string Sid { get; init; }
        public int Level { get; init; }
    }
}