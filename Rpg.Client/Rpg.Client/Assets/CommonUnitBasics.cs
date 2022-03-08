namespace Rpg.Client.Assets
{
    internal class CommonUnitBasics
    {
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

            BASE_MANA_POOL_SIZE = 3;
            MANA_PER_LEVEL = 1;
            COMBAT_RESTORE_SHARE = 1.0f;
            LEVEL_BASE = 2;
            LEVEL_MULTIPLICATOR = 100;
            OVERPOWER_BASE = 2;
            MINIMAL_LEVEL_WITH_MANA = 2;
        }

        public float ARMOR_BASE { get; set; }
        public float DAMAGE_BASE { get; set; }

        public float HERO_POWER_MULTIPLICATOR { get; set; }
        public int HITPOINTS_BASE { get; set; }
        public int HITPOINTS_PER_LEVEL_BASE { get; set; }
        public float POWER_BASE { get; set; }
        public float POWER_PER_LEVEL_BASE { get; set; }
        public float SUPPORT_BASE { get; set; }

        public int BASE_MANA_POOL_SIZE { get; set; }
        public int MANA_PER_LEVEL { get; set; }
        public float COMBAT_RESTORE_SHARE { get; set; }
        public int LEVEL_BASE { get; set; }
        public int LEVEL_MULTIPLICATOR { get; set; }
        public float OVERPOWER_BASE { get; set; }
        public int MINIMAL_LEVEL_WITH_MANA { get; set; }
    }
}