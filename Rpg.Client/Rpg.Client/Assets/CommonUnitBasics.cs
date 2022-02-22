namespace Rpg.Client.Assets
{
    internal class CommonUnitBasics
    {
        public int HITPOINTS_BASE { get; set; }
        public int HITPOINTS_PER_LEVEL_BASE { get; set; }

        public float ARMOR_BASE { get; set; }
        public float DAMAGE_BASE { get; set; }
        public float SUPPORT_BASE { get; set; }

        public float HERO_POWER_MULTIPLICATOR { get; set; }
        public float POWER_BASE { get; set; }
        public float POWER_PER_LEVEL_BASE { get; set; }

        public CommonUnitBasics()
        {
            HITPOINTS_BASE = 40;
            HITPOINTS_PER_LEVEL_BASE = HITPOINTS_BASE / 10;
            ARMOR_BASE = 0.5f;
            DAMAGE_BASE = 5;
            SUPPORT_BASE = 2;
            HERO_POWER_MULTIPLICATOR = 2.5f;
            POWER_BASE = 1f;
            POWER_PER_LEVEL_BASE = 0.1f;
        }
    }
}