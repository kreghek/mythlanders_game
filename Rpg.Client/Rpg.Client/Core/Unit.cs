using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class Unit
    {
        private const int BASE_MANA_POOL_SIZE = 3;
        private const int MANA_PER_LEVEL = 1;
        private const float COMBAT_RESTORE_SHARE = 1.0f;

        public Unit(UnitScheme unitScheme, int level) : this(unitScheme, level,
            equipmentLevel: 0,
            xp: 0,
            equipmentItems: 0)
        {
        }

        public Unit(UnitScheme unitScheme, int level, int equipmentLevel, int xp, int equipmentItems)
        {
            UnitScheme = unitScheme;

            Level = level;
            Xp = xp;
            EquipmentLevel = equipmentLevel;
            EquipmentItems = equipmentItems;

            InitStats(unitScheme);
            RestoreHp();

            ManaPool = 0;
        }

        public int EquipmentItems { get; private set; }

        public int EquipmentLevel { get; set; }

        public int EquipmentLevelup => (int)Math.Pow(2, EquipmentLevel);

        public int EquipmentRemains => EquipmentLevelup - EquipmentItems;

        public bool HasSkillsWithCost
        {
            get
            {
                var manaDependentSkills = Skills.Where(x => x.ManaCost is not null);
                return manaDependentSkills.Any();
            }
        }

        public int Hp { get; set; }

        public bool IsDead => Hp <= 0;

        public bool IsPlayerControlled { get; set; }

        public int Level { get; set; }

        private const int LEVEL_BASE = 2;
        private const int LEVEL_MULTIPLICATOR = 100;
        public int LevelupXp => (int)Math.Pow(LEVEL_BASE, Level) * LEVEL_MULTIPLICATOR;

        public int ManaPool { get; set; }
        public int ManaPoolSize => BASE_MANA_POOL_SIZE + (Level - 1) * MANA_PER_LEVEL;

        public int MaxHp { get; set; }

        public float Power => CalcPower();

        public int Armor => CalcArmor();

        public int Damage => CalcDamage();

        public int Support => CalcSupport();

        private int CalcSupport()
        {
            var power = Power;
            var powerToSupport = power * UnitScheme.SupportRank;
            var support = UnitScheme.SupportBase * powerToSupport;
            var normalizedSupport = (int)Math.Round(support, MidpointRounding.AwayFromZero);

            return normalizedSupport;
        }

        private int CalcDamage()
        {
            var power = Power;
            var powerToDamage = power * UnitScheme.DamageDealerRank;
            var damage = UnitScheme.DamageBase * powerToDamage;
            var normalizedDamage = (int)Math.Round(damage, MidpointRounding.AwayFromZero);

            return normalizedDamage;
        }

        private int CalcArmor()
        {
            var power = Power;
            var powerToArmor = power * UnitScheme.DamageDealerRank;
            var armor = UnitScheme.ArmorBase * powerToArmor;
            var normalizedArmor = (int)Math.Round(armor, MidpointRounding.AwayFromZero);

            return normalizedArmor;
        }

        private float CalcPower()
        {
            var powerLevel = CalcPowerLevel();
            var overpower = CalcOverpower();

            return UnitScheme.Power + UnitScheme.PowerPerLevel * powerLevel + overpower;
        }

        private const float OVERPOWER_BASE = 2;
        private const int MINIMAL_LEVEL_WITH_MANA = 2;

        private float CalcOverpower()
        {
            var startPoolSize = ManaPool - (BASE_MANA_POOL_SIZE + MANA_PER_LEVEL * MINIMAL_LEVEL_WITH_MANA);
            if (startPoolSize > 0)
            {
                return (float)Math.Log(startPoolSize, OVERPOWER_BASE);
            }

            // Monsters and low-level heroes has no overpower.
            return 0;
        }

        private float CalcPowerLevel()
        {
            float powerLevel;
            if (EquipmentLevel > 0)
            {
                powerLevel = (Level * 0.5f + EquipmentLevel * 0.5f);
            }
            else
            {
                // The monsters do not use equipment level. They has no equipment at all.
                powerLevel = Level;
            }

            return powerLevel;
        }

        public IReadOnlyList<ISkill> Skills { get; set; }

        public int SkillSetIndex
        {
            get
            {
                var skillSetIndex = 0;
                if (EquipmentLevel > 0)
                {
                    skillSetIndex = EquipmentLevel - 1;
                }

                var skillSetIndexNormalized = Math.Min(skillSetIndex, UnitScheme.SkillSets.Count - 1);

                return skillSetIndexNormalized;
            }
        }

        public UnitScheme UnitScheme { get; init; }

        public int Xp { get; set; }

        public int XpRemains => LevelupXp - Xp;

        /// <summary>
        /// Used only by monster units.
        /// Amount of the experience gained for killing this unit.
        /// </summary>
        public int XpReward => Level * 20;

        public bool GainEquipmentItem(int amount)
        {
            var items = EquipmentItems;
            var level = EquipmentLevel;
            var equipmentLevelup = EquipmentLevelup;
            var equipmentRemains = EquipmentRemains;
            var wasLevelUp = GainCounterInner(amount, ref items, ref level, ref equipmentLevelup, ref equipmentRemains);
            EquipmentItems = items;
            EquipmentLevel = level;

            if (wasLevelUp)
            {
                InitStats(UnitScheme);
            }

            return wasLevelUp;
        }

        /// <summary>
        /// Increase XP.
        /// </summary>
        /// <returns>Returns true is level up.</returns>
        public bool GainXp(int amount)
        {
            var xp = this.Xp;
            var level = this.Level;
            var levelupXp = this.LevelupXp;
            var xpRemains = this.XpRemains;
            var wasLevelUp = GainCounterInner(amount, ref xp, ref level, ref levelupXp, ref xpRemains);
            this.Xp = xp;
            this.Level = level;

            if (wasLevelUp)
            {
                InitStats(UnitScheme);
            }

            return wasLevelUp;
        }

        public void RestoreHitPointsAfterCombat()
        {
            var hpBonus = (int)Math.Round(MaxHp * COMBAT_RESTORE_SHARE, MidpointRounding.ToEven);

            Hp += hpBonus;

            if (Hp > MaxHp)
            {
                Hp = MaxHp;
            }
        }

        public int TakeDamage(CombatUnit damageDealer, int damage)
        {
            var damageAbsorbedByArmor = Math.Max(damage - Armor, 0);
            Hp -= Math.Min(Hp, damageAbsorbedByArmor);
            HasBeenDamaged?.Invoke(this, damageAbsorbedByArmor);
            if (Hp <= 0)
            {
                Dead?.Invoke(this, new UnitDamagedEventArgs(damageDealer));
            }

            return damageAbsorbedByArmor;
        }

        public void RestoreHitPoints(int heal)
        {
            Hp += Math.Min(MaxHp - Hp, heal);
            HealTaken?.Invoke(this, heal);
        }

        internal void RestoreManaPoint()
        {
            if (ManaPool < ManaPoolSize)
            {
                ManaPool++;
            }
        }

        private static bool GainCounterInner(int amount, ref int xp, ref int level, ref int levelupXp,
            ref int xpRemains)
        {
            var currentXpCounter = amount;
            var wasLevelup = false;

            while (currentXpCounter > 0)
            {
                var xpToNextLevel = Math.Min(currentXpCounter, xpRemains);
                currentXpCounter -= xpToNextLevel;

                xp += xpToNextLevel;

                if (xp >= levelupXp)
                {
                    level++;
                    xp = 0;

                    wasLevelup = true;
                }
            }

            return wasLevelup;
        }

        private void InitStats(UnitScheme unitScheme)
        {
            MaxHp = (int)Math.Round(unitScheme.HitPointsBase + unitScheme.HitPointsPerLevelBase * Level, MidpointRounding.AwayFromZero);

            Skills = unitScheme.SkillSets[SkillSetIndex].Skills;
        }

        private void RestoreHp()
        {
            Hp = MaxHp;
        }

        public event EventHandler<int>? HasBeenDamaged;

        public event EventHandler<int>? HealTaken;

        public event EventHandler<UnitDamagedEventArgs>? Dead;

        public sealed class UnitDamagedEventArgs : EventArgs
        {
            public UnitDamagedEventArgs(CombatUnit damageDealer)
            {
                DamageDealer = damageDealer ?? throw new ArgumentNullException(nameof(damageDealer));
            }

            public CombatUnit DamageDealer { get; }
        }
    }
}