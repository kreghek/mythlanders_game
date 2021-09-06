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
            CombatLevel = combatLevel;

            InitStats(unitScheme, combatLevel);
        }

        public int CombatLevel { get; set; }

        public int Hp { get; set; }

        public bool IsDead => Hp <= 0;

        public bool IsPlayerControlled { get; set; }
        public int MaxHp { get; set; }

        public IEnumerable<CombatSkill> Skills { get; set; }

        public UnitScheme UnitScheme { get; init; }

        public int Xp { get; set; }

        public void GainXp(int amount)
        {
            Xp += amount;

            var xpToLevel = 100 + CombatLevel * 100;
            if (Xp >= xpToLevel)
            {
                CombatLevel++;
                Xp = Xp - xpToLevel;

                InitStats(UnitScheme, CombatLevel);
            }
        }

        public void TakeDamage(int damage)
        {
            Hp -= Math.Min(Hp, damage);
            DamageTaken?.Invoke(this, EventArgs.Empty);
        }

        public void TakeHeal(int heal)
        {
            Hp += Math.Min(MaxHp - Hp, heal);
            HealTaken?.Invoke(this, EventArgs.Empty);
        }

        private void InitStats(UnitScheme unitScheme, int combatLevel)
        {
            MaxHp = unitScheme.Hp + unitScheme.HpPerLevel * CombatLevel;
            Hp = MaxHp;

            Skills = unitScheme.Skills.Select(x => new CombatSkill
            {
                DamageMin = x.DamageMin + x.DamageMinPerLevel * combatLevel,
                DamageMax = x.DamageMax + x.DamageMaxPerLevel * combatLevel,
                TargetType = x.TargetType,
                Scope = x.Scope
            }).ToArray();
        }

        public event EventHandler DamageTaken;

        public event EventHandler HealTaken;
    }
}