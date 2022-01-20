using System;

namespace Rpg.Client.Core.Perks
{
    internal sealed class BossMonster : IPerk
    {
        private readonly int _bossLevel;
        
        private const float HITPOINTS_BONUS = 3.5f;
        private const float ARMOR_BONUS = 2f;
        
        private const float BOSS_POWER_MULTIPLICATOR = 4.5f;

        public BossMonster(int bossLevel)
        {
            _bossLevel = bossLevel;
        }

        public void ApplyToStats(ref float maxHitpoints, ref float armorBonus)
        {
            maxHitpoints = (float)Math.Round(maxHitpoints * HITPOINTS_BONUS * _bossLevel * BOSS_POWER_MULTIPLICATOR);
            armorBonus = ARMOR_BONUS * _bossLevel * _bossLevel * BOSS_POWER_MULTIPLICATOR;
        }
        
        public int ModifyDamage(int sourceDamage, IDice dice)
        {
            return (int)Math.Round(sourceDamage * _bossLevel * BOSS_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
        }
    }
}