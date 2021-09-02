using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal class Unit
    {
        private int _combatLevel;

        public Unit(UnitScheme unitScheme, int combatLevel)
        {
            UnitScheme = unitScheme;
            _combatLevel = combatLevel;

            InitStats(unitScheme, combatLevel);
        }

        private void InitStats(UnitScheme unitScheme, int combatLevel)
        {
            MaxHp = unitScheme.Hp + unitScheme.HpPerLevel * _combatLevel;
            Hp = MaxHp;

            Skills = unitScheme.Skills.Select(x => new CombatSkill
            {
                DamageMin = x.DamageMin + x.DamageMinPerLevel * combatLevel,
                DamageMax = x.DamageMax + x.DamageMaxPerLevel * combatLevel,
            }).ToArray();
        }

        public UnitScheme UnitScheme { get; init; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }

        public int Xp { get; set; }

        public IEnumerable<CombatSkill> Skills { get; set; }

        public bool IsPlayerControlled { get; set; }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
            DamageTaken?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler DamageTaken;

        public bool IsDead => Hp <= 0;

        public int CombatLevel { get => _combatLevel; set => _combatLevel = value; }

        public void GainXp(int amount)
        {
            Xp += amount;

            var xpToLevel = 100 + _combatLevel * 100;
            if (Xp >= xpToLevel)
            {
                _combatLevel++;
                Xp = Xp - xpToLevel;

                InitStats(UnitScheme, _combatLevel);
            }
        }
    }
}