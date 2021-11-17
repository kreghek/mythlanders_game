using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class UnitScheme
    {
        public BiomeType Biome { get; init; }

        public float TankRole { get; init; }
        public float DamageDealerRole { get; init; }
        public float SupportRole { get; init; }

        public int HitPoints => CalcHitPoints();

        private const int HITPOINTS_BASE = 20;
        private const int HITPOINTS_PER_LEVEL = HITPOINTS_BASE / 10;
            
        private int CalcHitPoints()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(HITPOINTS_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            return HITPOINTS_BASE;
        }

        public int HitPointsPerLevel => CalcHitPointsPerLevel();

        private int CalcHitPointsPerLevel()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(HITPOINTS_PER_LEVEL * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            return HITPOINTS_PER_LEVEL;
        }

        public float ArmorBase => CalcArmor();

        public float DamageBase => CalcDamage();

        public float SupportBase => CalcSupport();

        private float CalcSupport()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(SUPPORT_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            return SUPPORT_BASE;
        }

        private float CalcDamage()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(DAMAGE_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            return DAMAGE_BASE;
        }

        private const float ARMOR_BASE = 0.5f;
        private const float DAMAGE_BASE = 3;
        private const float SUPPORT_BASE = 2;
        
        private float CalcArmor()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(ARMOR_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            return ARMOR_BASE;
        }

        public bool IsBoss { get; init; }

        public bool IsUnique { get; init; }

        public UnitName Name { get; init; }

        public IEnumerable<int>? NodeIndexes { get; init; }

        public bool IsMonster { get; init; }

        public float Power => CalcPower();

        private const float HERO_POWER_MULTIPLICATOR = 3f;
        private const float BASE_POWER = 1f;
        private const float POWER_PER_LEVEL = 0.1f;
        
        private float CalcPower()
        {
            if (!IsMonster)
            {
                return BASE_POWER * HERO_POWER_MULTIPLICATOR;
            }

            return BASE_POWER;
        }

        public float PowerPerLevel => CalcPowerPerLevel();

        private float CalcPowerPerLevel()
        {
            if (!IsMonster)
            {
                return POWER_PER_LEVEL * HERO_POWER_MULTIPLICATOR;
            }

            return POWER_PER_LEVEL;
        }

        public UnitSchemeAutoTransition? SchemeAutoTransition { get; init; }

        public IReadOnlyList<SkillSet> SkillSets { get; init; }

        public UnitGraphicsConfigBase UnitGraphicsConfig { get; init; }
    }
}