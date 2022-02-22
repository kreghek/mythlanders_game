namespace BalanceConverter
{
    internal class UnitBasics
    {
        public float ARMOR_BASE { get; set; }
        public float DAMAGE_BASE { get; set; }

        public float HERO_POWER_MULTIPLICATOR { get; set; }
        public int HITPOINTS_BASE { get; set; }
        public int HITPOINTS_PER_LEVEL_BASE { get; set; }
        public float POWER_BASE { get; set; }
        public float POWER_PER_LEVEL_BASE { get; set; }
        public float SUPPORT_BASE { get; set; }
    }

    internal class BalanceData
    {
        public UnitBasics UnitBasics { get; init; }
        public UnitRow[] UnitRows { get; init; }
    }
}