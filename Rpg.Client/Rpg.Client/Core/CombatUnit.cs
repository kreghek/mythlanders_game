using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal enum CombatUnitState
    { 
        Idle,
        Defense
    }

    internal sealed class CombatUnit
    {
        public CombatUnit(Unit unit, int index)
        {
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            Index = index;
            var skillContext = new CombatSkillContext(this);

            var cards = unit.Skills.Select(skill => new CombatSkillCard(skill, skillContext)).ToList();

            CombatCards = cards;

            unit.HasBeenDamaged += Unit_HasBeenDamaged;
            unit.HealTaken += Unit_HealTaken;
        }

        public IEnumerable<CombatSkillCard> CombatCards { get; }

        public int Index { get; }

        public Unit Unit { get; }
        public CombatUnitState State { get; internal set; }

        private void Unit_HasBeenDamaged(object? sender, int e)
        {
            HasTakenDamage?.Invoke(this, new UnitHpChangedEventArgs { CombatUnit = this, Amount = e });
        }

        private void Unit_HealTaken(object? sender, int e)
        {
            Healed?.Invoke(this, new UnitHpChangedEventArgs { CombatUnit = this, Amount = e });
        }

        internal event EventHandler<UnitHpChangedEventArgs>? HasTakenDamage;
        internal event EventHandler<UnitHpChangedEventArgs>? Healed;

        internal class UnitHpChangedEventArgs : EventArgs
        {
            public int Amount { get; set; }
            public CombatUnit CombatUnit { get; set; }
        }

        internal void ChangeState(CombatUnitState targetState)
        {
            State = targetState;
        }
    }
}