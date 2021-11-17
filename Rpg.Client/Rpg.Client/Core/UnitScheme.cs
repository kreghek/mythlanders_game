using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class UnitScheme
    {
        public BiomeType Biome { get; init; }

        public float TankRank { get; init; }
        public float DamageDealerRank { get; init; }
        public float SupportRank { get; init; }

        public float HitPointsBase => CalcHitPointsBase();

        private const int HITPOINTS_BASE = 40;
        private const int HITPOINTS_PER_LEVEL_BASE = HITPOINTS_BASE / 10;
            
        private float CalcHitPointsBase()
        {
            if (!IsMonster)
            {
                return (float)Math.Round(HITPOINTS_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }
            else
            {
                if (BossLevel is null)
                {
                    return HITPOINTS_BASE;
                }
                else
                {
                    return HITPOINTS_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
                }
            }
        }

        public float HitPointsPerLevelBase => CalcHitPointsPerLevelBase();

        private float CalcHitPointsPerLevelBase()
        {
            if (!IsMonster)
            {
                return (float)Math.Round(HITPOINTS_PER_LEVEL_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }
            else
            {
                if (BossLevel is null)
                {
                    return HITPOINTS_PER_LEVEL_BASE;
                }
                else
                {
                    return HITPOINTS_PER_LEVEL_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
                }
            }
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
            else
            {
                if (BossLevel is null)
                {
                    return SUPPORT_BASE;
                }
                else
                {
                    return SUPPORT_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
                }
            }
        }

        private float CalcDamage()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(DAMAGE_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }
            else
            {
                if (BossLevel is null)
                {
                    return DAMAGE_BASE;
                }
                else
                {
                    return DAMAGE_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
                }
            }
        }

        private const float ARMOR_BASE = 0.5f;
        private const float DAMAGE_BASE = 5;
        private const float SUPPORT_BASE = 2;
        
        private float CalcArmor()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(ARMOR_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }
            else
            {
                if (BossLevel is null)
                {
                    return ARMOR_BASE;
                }
                else
                {
                    return ARMOR_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
                }
            }
        }

        public int? BossLevel { get; init; }

        public bool IsUnique { get; init; }

        public UnitName Name { get; init; }

        public IEnumerable<int>? NodeIndexes { get; init; }

        public bool IsMonster { get; init; }

        public float Power => CalcPower();

        private const float HERO_POWER_MULTIPLICATOR = 2.5f;
        private const float BOSS_POWER_MULTIPLICATOR = 4.5f;
        private const float POWER_BASE = 1f;
        private const float POWER_PER_LEVEL_BASE = 0.1f;
        
        private float CalcPower()
        {
            if (!IsMonster)
            {
                return POWER_BASE * HERO_POWER_MULTIPLICATOR;
            }
            else
            {
                if (BossLevel is null)
                {
                    return POWER_BASE;
                }
                else
                {
                    return POWER_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
                }
            }
        }

        public float PowerPerLevel => CalcPowerPerLevel();

        private float CalcPowerPerLevel()
        {
            if (!IsMonster)
            {
                return POWER_PER_LEVEL_BASE * HERO_POWER_MULTIPLICATOR;
            }
            else
            {
                if (BossLevel is null)
                {
                    return POWER_PER_LEVEL_BASE;
                }
                else
                {
                    return POWER_PER_LEVEL_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
                }
            }
        }

        public UnitSchemeAutoTransition? SchemeAutoTransition { get; init; }

        public IReadOnlyList<SkillSet> SkillSets { get; init; }

        public UnitGraphicsConfigBase UnitGraphicsConfig { get; init; }
    }
}