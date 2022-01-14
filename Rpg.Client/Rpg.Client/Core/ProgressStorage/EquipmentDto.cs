namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record EquipmentDto
    {
        public int Level { get; init; }
        public string Sid { get; init; }
    }
}