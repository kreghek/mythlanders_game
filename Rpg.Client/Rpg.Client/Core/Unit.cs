using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal class Unit
    {
        public Unit(UnitScheme unitScheme, int combatLevel)
        {
            UnitScheme = unitScheme;
            Level = combatLevel;

            InitStats(unitScheme, combatLevel);
        }

        public int Hp { get; set; }

        public bool IsDead => Hp <= 0;

        public bool IsPlayerControlled { get; set; }

        public int Level { get; set; }
        public int MaxHp { get; set; }

        public IEnumerable<CombatSkill> Skills { get; set; }

        public UnitScheme UnitScheme { get; init; }

        public int Xp { get; set; }

        public int XpReward => Level * 20;

        public int XpToLevelup => 100 + Level * 100;

        /// <summary>
        /// Increase XP.
        /// </summary>
        /// <returns>Returns true is level up.</returns>
        public bool GainXp(int amount)
        {
            Xp += amount;

            var xpToLevel = XpToLevelup;
            if (Xp >= xpToLevel)
            {
                Level++;
                Xp -= xpToLevel;

                InitStats(UnitScheme, Level);

                return true;
            }

            return false;
        }

        public void TakeDamage(int damage)
        {
            Hp -= Math.Min(Hp, damage);
            DamageTaken?.Invoke(this, damage);
        }

        public void TakeHeal(int heal)
        {
            Hp += Math.Min(MaxHp - Hp, heal);
            HealTaken?.Invoke(this, heal);
        }

        private void InitStats(UnitScheme unitScheme, int combatLevel)
        {
            MaxHp = unitScheme.Hp + unitScheme.HpPerLevel * Level;
            Hp = MaxHp;

            Skills = unitScheme.Skills.Select(x => new CombatSkill
            {
                Sid = x.Sid,
                DamageMin = x.DamageMin + x.DamageMinPerLevel * combatLevel,
                DamageMax = x.DamageMax + x.DamageMaxPerLevel * combatLevel,
                TargetType = x.TargetType,
                Scope = x.Scope,
                Range = x.Range
            }).ToArray();
        }

        public event EventHandler<int> DamageTaken;

        public event EventHandler<int> HealTaken;
    }
}