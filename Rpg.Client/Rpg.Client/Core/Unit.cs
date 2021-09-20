using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal class Unit
    {
        public Unit(UnitScheme unitScheme, int combatLevel)
        {
            UnitScheme = unitScheme;
            Level = combatLevel;

            InitStats(unitScheme, combatLevel - 1);
        }

        public int Hp { get; set; }

        public bool IsDead => Hp <= 0;

        public bool IsPlayerControlled { get; set; }

        public int Level { get; set; }
        public int MaxHp { get; set; }

        public int Power { get; set; }
        public int PowerIncrease { get; set; }

        public IEnumerable<SkillBase> Skills { get; set; }

        public UnitScheme UnitScheme { get; init; }

        public int Xp { get; set; }

        /// <summary>
        /// Used only by monster units.
        /// Amount of the expirience gained for killing this unit.
        /// </summary>
        public int XpReward => Level * 20;

        public int LevelupXp => 100 + Level * 100;

        public int XpRemains => LevelupXp - Xp;

        /// <summary>
        /// Increase XP.
        /// </summary>
        /// <returns>Returns true is level up.</returns>
        public bool GainXp(int amount)
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

                    InitStats(UnitScheme, Level - 1);

                    wasLevelup = true;
                }
            }

            return wasLevelup;
        }

        public void TakeDamage(int damage)
        {
            Hp -= Math.Min(Hp, damage);
            DamageTaken?.Invoke(this, damage);
            if (Hp <= 0)
            {
                Dead?.Invoke(this, new EventArgs());
            }
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

            PowerIncrease = unitScheme.PowerPerLevel;
            Power = unitScheme.Power + PowerIncrease * Level;

            Skills = unitScheme.Skills;
            //    .Select(x => new CombatSkill
            //{
            //    Sid = x.Sid,
            //    DamageMin = x.DamageMin + x.DamageMinPerLevel * combatLevel,
            //    DamageMax = x.DamageMax + x.DamageMaxPerLevel * combatLevel,
            //    TargetType = x.TargetType,
            //    Scope = x.Scope,
            //    Range = x.Range
            //}).ToArray();
        }

        public event EventHandler<int>? DamageTaken;

        public event EventHandler<int>? HealTaken;

        public event EventHandler? Dead;
    }
}