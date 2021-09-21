namespace Rpg.Client.Core.ProgressStorage
{

    internal sealed record UnitDto
    {
        public string SchemeSid { get; init; }

        public int Hp { get; init; }

        public int Xp { get; init; }

        public int Level { get; init; }

        public int EquipmentLevel { get; init; }
        public int EquipmentItems { get; set; }
    }
}