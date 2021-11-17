using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class UnitScheme
    {
        private const int HITPOINTS_BASE = 40;
        private const int HITPOINTS_PER_LEVEL_BASE = HITPOINTS_BASE / 10;

        private const float ARMOR_BASE = 0.5f;
        private const float DAMAGE_BASE = 5;
        private const float SUPPORT_BASE = 2;

        private const float HERO_POWER_MULTIPLICATOR = 2.5f;
        private const float BOSS_POWER_MULTIPLICATOR = 4.5f;
        private const float POWER_BASE = 1f;
        private const float POWER_PER_LEVEL_BASE = 0.1f;

        public float ArmorBase => CalcArmor();
        public BiomeType Biome { get; init; }

        public int? BossLevel { get; init; }

        public float DamageBase => CalcDamage();
        public float DamageDealerRank { get; init; }

        public float HitPointsBase => CalcHitPointsBase();

        public float HitPointsPerLevelBase => CalcHitPointsPerLevelBase();

        public bool IsMonster { get; init; }

        public bool IsUnique { get; init; }

        public UnitName Name { get; init; }

        public IEnumerable<int>? NodeIndexes { get; init; }

        public float Power => CalcPower();

        public float PowerPerLevel => CalcPowerPerLevel();

        public UnitSchemeAutoTransition? SchemeAutoTransition { get; init; }

        public IReadOnlyList<SkillSet> SkillSets { get; init; }

        public float SupportBase => CalcSupport();
        public float SupportRank { get; init; }

        public float TankRank { get; init; }

        public UnitGraphicsConfigBase UnitGraphicsConfig { get; init; }

        private float CalcArmor()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(ARMOR_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            if (BossLevel is null)
            {
                return ARMOR_BASE;
            }

            return ARMOR_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
        }

        private float CalcDamage()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(DAMAGE_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            if (BossLevel is null)
            {
                return DAMAGE_BASE;
            }

            return DAMAGE_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
        }

        private float CalcHitPointsBase()
        {
            if (!IsMonster)
            {
                return (float)Math.Round(HITPOINTS_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            if (BossLevel is null)
            {
                return HITPOINTS_BASE;
            }

            return HITPOINTS_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
        }

        private float CalcHitPointsPerLevelBase()
        {
            if (!IsMonster)
            {
                return (float)Math.Round(HITPOINTS_PER_LEVEL_BASE * HERO_POWER_MULTIPLICATOR,
                    MidpointRounding.AwayFromZero);
            }

            if (BossLevel is null)
            {
                return HITPOINTS_PER_LEVEL_BASE;
            }

            return HITPOINTS_PER_LEVEL_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
        }

        private float CalcPower()
        {
            if (!IsMonster)
            {
                return POWER_BASE * HERO_POWER_MULTIPLICATOR;
            }

            if (BossLevel is null)
            {
                return POWER_BASE;
            }

            return POWER_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
        }

        private float CalcPowerPerLevel()
        {
            if (!IsMonster)
            {
                return POWER_PER_LEVEL_BASE * HERO_POWER_MULTIPLICATOR;
            }

            if (BossLevel is null)
            {
                return POWER_PER_LEVEL_BASE;
            }

            return POWER_PER_LEVEL_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
        }

        private float CalcSupport()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(SUPPORT_BASE * HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            if (BossLevel is null)
            {
                return SUPPORT_BASE;
            }

            return SUPPORT_BASE * BOSS_POWER_MULTIPLICATOR * BossLevel.Value;
        }
    }
}