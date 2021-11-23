using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public CombatUnitState State { get; internal set; }

        public Unit Unit { get; }

        internal void ChangeState(CombatUnitState targetState)
        {
            State = targetState;
        }

        private void Unit_HasBeenDamaged(object? sender, UnitHasBeenDamagedEventArgs e)
        {
            Debug.Assert(e.Result is not null);
            Debug.Assert(e.Result?.ValueFinal is not null);
            var args = new UnitHitPointsChangedEventArgs
            {
                CombatUnit = this,
                Amount = e.Result.ValueFinal.Value,
                SourceAmount = e.Result.ValueSource,
                Direction = HitPointsChangeDirection.Negative
            };
            HasTakenDamage?.Invoke(this, args);
        }

        private void Unit_HealTaken(object? sender, int e)
        {
            Healed?.Invoke(this, new UnitHitPointsChangedEventArgs { CombatUnit = this, Amount = e });
        }

        internal event EventHandler<UnitHitPointsChangedEventArgs>? HasTakenDamage;
        internal event EventHandler<UnitHitPointsChangedEventArgs>? Healed;
    }
    
    internal class UnitHitPointsChangedEventArgs : EventArgs
    {
        public int Amount { get; init; }
        public int SourceAmount { get; init; }
        public CombatUnit? CombatUnit { get; init; }

        public HitPointsChangeDirection Direction { get; init; }
    }

    internal enum HitPointsChangeDirection
    {
        Positive,
        Negative
    }
}