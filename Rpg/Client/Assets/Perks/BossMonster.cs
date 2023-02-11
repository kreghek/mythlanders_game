using System;

using Client.Core;

using Core.Dices;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Perks
{
    internal sealed class BossMonster : IPerk
    {
        private const float HITPOINTS_BONUS = 3.5f;
        private const float ARMOR_BONUS = 2f;

        private const float BOSS_POWER_MULTIPLICATOR = 4.5f;
        private readonly int _bossLevel;

        public BossMonster()
        {
            _bossLevel = 1;
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