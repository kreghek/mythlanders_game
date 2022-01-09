namespace Rpg.Client.Core.ProgressStorage
{
    internal sealed record PlayerUnitDto
    {
        public int EquipmentLevel { get; init; }

        public int Hp { get; init; }

        public int Level { get; init; }

        public int? ManaPool { get; init; }

        public string SchemeSid { get; init; }
    }
}