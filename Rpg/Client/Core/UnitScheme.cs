using System;
using System.Collections.Generic;

using Core.Balance;

namespace Rpg.Client.Core
{
    internal sealed class UnitScheme
    {
        public UnitScheme(CommonUnitBasics unitBasics)
        {
            UnitBasics = unitBasics;

            DamageDealerRank = 1;

            Resolve = 10;
        }

        public float ArmorBase => CalcArmor();

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

        public int Resolve { get; init; }

        public UnitSchemeAutoTransition? SchemeAutoTransition { get; init; }

        public float SupportBase => CalcSupport();
        public float SupportRank { get; init; }

        public float TankRank { get; init; }

        public UnitGraphicsConfigBase? UnitGraphicsConfig { get; init; }

        internal CommonUnitBasics UnitBasics { get; }

        private float CalcArmor()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(UnitBasics.ARMOR_BASE * UnitBasics.HERO_POWER_MULTIPLICATOR,
                    MidpointRounding.AwayFromZero);
            }

            return UnitBasics.ARMOR_BASE;
        }

        private float CalcDamage()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(UnitBasics.DAMAGE_BASE * UnitBasics.HERO_POWER_MULTIPLICATOR,
                    MidpointRounding.AwayFromZero);
            }

            return UnitBasics.DAMAGE_BASE;
        }

        private float CalcHitPointsBase()
        {
            if (!IsMonster)
            {
                return (float)Math.Round(UnitBasics.HITPOINTS_BASE * UnitBasics.HERO_POWER_MULTIPLICATOR,
                    MidpointRounding.AwayFromZero);
            }

            return UnitBasics.HITPOINTS_BASE;
        }

        private float CalcHitPointsPerLevelBase()
        {
            if (!IsMonster)
            {
                return (float)Math.Round(UnitBasics.HITPOINTS_PER_LEVEL_BASE * UnitBasics.HERO_POWER_MULTIPLICATOR,
                    MidpointRounding.AwayFromZero);
            }

            return UnitBasics.HITPOINTS_PER_LEVEL_BASE;
        }

        private float CalcPower()
        {
            if (!IsMonster)
            {
                return UnitBasics.POWER_BASE * UnitBasics.HERO_POWER_MULTIPLICATOR;
            }

            return UnitBasics.POWER_BASE;
        }

        private float CalcPowerPerLevel()
        {
            if (!IsMonster)
            {
                return UnitBasics.POWER_PER_LEVEL_BASE * UnitBasics.HERO_POWER_MULTIPLICATOR;
            }

            return UnitBasics.POWER_PER_LEVEL_BASE;
        }

        private float CalcSupport()
        {
            if (!IsMonster)
            {
                return (int)Math.Round(UnitBasics.SUPPORT_BASE * UnitBasics.HERO_POWER_MULTIPLICATOR,
                    MidpointRounding.AwayFromZero);
            }

            return UnitBasics.SUPPORT_BASE;
        }
    }
}