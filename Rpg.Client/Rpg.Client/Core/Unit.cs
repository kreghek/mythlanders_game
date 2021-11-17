using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class Unit
    {
        private const int BASE_MANA_POOL_SIZE = 10;
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

            ManaPool = ManaPoolSize;
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

        public int LevelupXp => (int)Math.Pow(2, Level) * 100;

        public int ManaPool { get; set; }
        public int ManaPoolSize => BASE_MANA_POOL_SIZE + (Level - 1) * MANA_PER_LEVEL;

        public int MaxHp { get; set; }

        public float Power => CalcPower();

        private float CalcPower()
        {
            PowerIncrease = UnitScheme.PowerPerLevel;

            var powerLevel = CalcPowerLevel();
            var overpower = CalcOverpower();

            return UnitScheme.Power + PowerIncrease * powerLevel + overpower;
        }

        private const float OVERPOWER_BASE = 2;
        private float CalcOverpower()
        {
            if (ManaPoolSize > 0)
            {
                return (float)Math.Log(ManaPool, OVERPOWER_BASE);
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

        public int PowerIncrease { get; set; }

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

        public void TakeDamage(CombatUnit damageDealer, int damage)
        {
            Hp -= Math.Min(Hp, damage);
            HasBeenDamaged?.Invoke(this, damage);
            if (Hp <= 0)
            {
                Dead?.Invoke(this, new UnitDamagedEventArgs(damageDealer));
            }
        }

        public void TakeHeal(int heal)
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
            MaxHp = unitScheme.Hp + unitScheme.HpPerLevel * Level;

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