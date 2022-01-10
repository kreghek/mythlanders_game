﻿using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.Skills;

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

        public bool IsBig { get; init; }

        public IEnumerable<GlobeNodeSid>? LocationSids { get; init; }

        public int? MinRequiredBiomeLevel { get; init; }

        public UnitName Name { get; init; }

        public float Power => CalcPower();

        public float PowerPerLevel => CalcPowerPerLevel();

        public UnitSchemeAutoTransition? SchemeAutoTransition { get; init; }

        public float SupportBase => CalcSupport();
        public float SupportRank { get; init; }

        public float TankRank { get; init; }

        public UnitGraphicsConfigBase? UnitGraphicsConfig { get; init; }

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

            return DAMAGE_BASE * BossLevel.Value;
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

        public IReadOnlyList<IUnitLevelScheme>? Levels { get; init; }
    }

    internal interface IUnitLevelScheme
    {
        void Apply(Unit unit);
        int Level { get; }
    }

    internal abstract class UnitLevelBase : IUnitLevelScheme
    {
        public abstract void Apply(Unit unit);

        public int Level { get; }

        protected UnitLevelBase(int level)
        {
            Level = level;
        }
    }

    internal sealed class AddSkillUnitLevel : UnitLevelBase
    {
        private readonly ISkill _skill;

        public AddSkillUnitLevel(int level, ISkill skill) : base(level)
        {
            _skill = skill;
        }

        public override void Apply(Unit unit)
        {
            unit.Skills.Add(_skill);
        }
    }

    internal sealed class AddPerkUnitLevel : UnitLevelBase
    {
        private readonly IPerk _perk;

        public AddPerkUnitLevel(int level, IPerk perk) : base(level)
        {
            _perk = perk;
        }

        public override void Apply(Unit unit)
        {
            unit.Perks.Add(_perk);
        }
    }
    
    internal sealed class ReplaceSkillUnitLevel: UnitLevelBase
    {
        private readonly SkillSid _targetSid;
        private readonly ISkill _newSkill;

        public ReplaceSkillUnitLevel(int level, SkillSid targetSid, ISkill newSkill) : base(level)
        {
            _targetSid = targetSid;
            _newSkill = newSkill;
        }

        public override void Apply(Unit unit)
        {
            var targetSkill = unit.Skills.Single(x => x.Sid == _targetSid);
            var index = unit.Skills.IndexOf(targetSkill);
            unit.Skills[index] = _newSkill;
        }
    }
}