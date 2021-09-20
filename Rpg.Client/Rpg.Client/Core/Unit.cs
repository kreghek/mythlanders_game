using System;
using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal class Unit
    {
        private readonly IList<IUnitModifier> _unitModifiers;

        public Unit(UnitScheme unitScheme, int combatLevel)
        {
            _unitModifiers = new List<IUnitModifier>();

            UnitScheme = unitScheme;
            Level = combatLevel;

            InitStats(unitScheme);
            RestoreHP();
        }

        public int EquipmentItems { get; private set; }

        public int EquipmentLevel { get; set; }

        internal void RemoveModifier(PowerUpModifier modifier)
        {
            _unitModifiers.Remove(modifier);
            InitStats(UnitScheme);
        }

        public int EquipmentLevelup => (int)Math.Pow(2, EquipmentLevel);

        internal void AddModifier(PowerUpModifier modifier)
        {
            if (!_unitModifiers.Contains(modifier))
            {
                _unitModifiers.Add(modifier);
            }

            InitStats(UnitScheme);
        }

        public int EquipmentRemains => EquipmentLevelup - EquipmentItems;

        public int Hp { get; set; }

        public bool IsDead => Hp <= 0;

        public bool IsPlayerControlled { get; set; }

        public int Level { get; set; }

        public int LevelupXp => (int)Math.Pow(2, Level) * 100;

        public int MaxHp { get; set; }

        public int Power { get; set; }
        public int PowerIncrease { get; set; }

        public IEnumerable<SkillBase> Skills { get; set; }

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

                ApplyModifiers();
            }
            else
            {
                Power = unitScheme.Power + PowerIncrease * Level;

                ApplyModifiers();
            }

            Skills = unitScheme.Skills;
        }

        private void ApplyModifiers()
        {
            foreach (var modifier in _unitModifiers)
            {
                switch (modifier.Type)
                {
                    case ModifierType.Power:
                        Power = (int)Math.Round(Power * 1.1f, MidpointRounding.ToEven);
                        break;
                }
            }
        }

        private void RestoreHP()
        {
            Hp = MaxHp;
        }

        public event EventHandler<int>? DamageTaken;

        public event EventHandler<int>? HealTaken;

        public event EventHandler? Dead;
    }
}