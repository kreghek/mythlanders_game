﻿using System;
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

        public Unit(UnitScheme unitScheme, int level) : this(unitScheme, level, 0, 0, 0)
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
            RestoreHP();

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

        public int Power { get; set; }

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
        /// Amount of the expirience gained for killing this unit.
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
            var Xp = this.Xp;
            var Level = this.Level;
            var LevelupXp = this.LevelupXp;
            var XpRemains = this.XpRemains;
            var wasLevelUp = GainCounterInner(amount, ref Xp, ref Level, ref LevelupXp, ref XpRemains);
            this.Xp = Xp;
            this.Level = Level;

            if (wasLevelUp)
            {
                InitStats(UnitScheme);
            }

            return wasLevelUp;
        }

        public void RestoreHitpointsAfterCombat()
        {
            var hpBonus = (int)Math.Round(MaxHp * COMBAT_RESTORE_SHARE, MidpointRounding.ToEven);

            Hp += hpBonus;

            if (Hp > MaxHp)
            {
                Hp = MaxHp;
            }
        }

        public void TakeDamage(CombatUnit damager, int damage)
        {
            Hp -= Math.Min(Hp, damage);
            HasBeenDamaged?.Invoke(this, damage);
            if (Hp <= 0)
            {
                Dead?.Invoke(this, new UnitDamagedEventArgs(damager));
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

        private static bool GainCounterInner(int amount, ref int Xp, ref int Level, ref int LevelupXp,
            ref int XpRemains)
        {
            var currentXpCounter = amount;
            var wasLevelup = false;

            while (currentXpCounter > 0)
            {
                var xpToNextLevel = Math.Min(currentXpCounter, XpRemains);
                currentXpCounter -= xpToNextLevel;

                Xp += xpToNextLevel;

                if (Xp >= LevelupXp)
                {
                    Level++;
                    Xp = 0;

                    wasLevelup = true;
                }
            }

            return wasLevelup;
        }

        private void InitStats(UnitScheme unitScheme)
        {
            MaxHp = unitScheme.Hp + unitScheme.HpPerLevel * Level;

            PowerIncrease = unitScheme.PowerPerLevel;

            if (EquipmentLevel > 0)
            {
                Power = unitScheme.Power + (int)Math.Round(PowerIncrease * (Level * 0.5f + EquipmentLevel * 0.5f),
                    MidpointRounding.AwayFromZero);
            }
            else
            {
                Power = unitScheme.Power + PowerIncrease * Level;
            }

            Skills = unitScheme.SkillSets[SkillSetIndex].Skills;
        }

        private void RestoreHP()
        {
            Hp = MaxHp;
        }

        public event EventHandler<int>? HasBeenDamaged;

        public event EventHandler<int>? HealTaken;

        public event EventHandler<UnitDamagedEventArgs>? Dead;

        public sealed class UnitDamagedEventArgs : EventArgs
        {
            public UnitDamagedEventArgs(CombatUnit damager)
            {
                Damager = damager ?? throw new ArgumentNullException(nameof(damager));
            }

            public CombatUnit Damager { get; }
        }
    }
}