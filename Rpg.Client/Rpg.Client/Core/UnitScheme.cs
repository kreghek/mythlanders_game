using System;
using System.Collections.Generic;

using Rpg.Client.Assets;

namespace Rpg.Client.Core
{
    internal sealed class UnitScheme
    {
        private readonly CommonUnitBasics _unitBasics;

        public UnitScheme(CommonUnitBasics unitBasics)
        {
            _unitBasics = unitBasics;
        }

        public float ArmorBase => CalcArmor();
        public BiomeType Biome { get; init; }

        public float DamageBase => CalcDamage();
        public float DamageDealerRank { get; init; }

        // Null for monsters
        public IReadOnlyList<IEquipmentScheme>? Equipments { get; init; }

        public float HitPointsBase => CalcHitPointsBase();

        public float HitPointsPerLevelBase => CalcHitPointsPerLevelBase();

        public bool IsMonster { get; init; }

        public bool IsUnique { get; init; }

        public IReadOnlyList<IUnitLevelScheme>? Levels { get; init; }

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
                return (int)Math.Round(_unitBasics.ARMOR_BASE * _unitBasics.HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            return _unitBasics.ARMOR_BASE;
        }

        private float CalcDamage()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(_unitBasics.DAMAGE_BASE * _unitBasics.HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            return _unitBasics.DAMAGE_BASE;
        }

        private float CalcHitPointsBase()
        {
            if (!IsMonster)
            {
                return (float)Math.Round(_unitBasics.HITPOINTS_BASE * _unitBasics.HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            return _unitBasics.HITPOINTS_BASE;
        }

        private float CalcHitPointsPerLevelBase()
        {
            if (!IsMonster)
            {
                return (float)Math.Round(_unitBasics.HITPOINTS_PER_LEVEL_BASE * _unitBasics.HERO_POWER_MULTIPLICATOR,
                    MidpointRounding.AwayFromZero);
            }

            return _unitBasics.HITPOINTS_PER_LEVEL_BASE;
        }

        private float CalcPower()
        {
            if (!IsMonster)
            {
                return _unitBasics.POWER_BASE * _unitBasics.HERO_POWER_MULTIPLICATOR;
            }

            return _unitBasics.POWER_BASE;
        }

        private float CalcPowerPerLevel()
        {
            if (!IsMonster)
            {
                return _unitBasics.POWER_PER_LEVEL_BASE * _unitBasics.HERO_POWER_MULTIPLICATOR;
            }

            return _unitBasics.POWER_PER_LEVEL_BASE;
        }

        private float CalcSupport()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(_unitBasics.SUPPORT_BASE * _unitBasics.HERO_POWER_MULTIPLICATOR, MidpointRounding.AwayFromZero);
            }

            return _unitBasics.SUPPORT_BASE;
        }
    }
}