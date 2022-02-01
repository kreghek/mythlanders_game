using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class VoiceCombatUnit: ICombatUnit
    {
        public Unit Unit { get; }
        public void ChangeState(CombatUnitState targetState)
        {
        }

        public event EventHandler<UnitHitPointsChangedEventArgs>? HasTakenDamage;
    }

    internal sealed class CombatUnit : ICombatUnit
    {
        public CombatUnit(Unit unit, GroupSlot slot)
        {
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            Index = slot.Index;
            IsInTankLine = slot.IsTankLine;
            var skillContext = new CombatSkillContext(this);

            var cards = unit.Skills.Select(skill => new CombatSkill(skill, skillContext)).ToList();

            CombatCards = cards;

            unit.HasBeenDamaged += Unit_HasBeenDamaged;
            unit.HasBeenHealed += Unit_BeenHealed;
            unit.HasAvoidedDamage += Unit_HasAvoidedDamage;
        }

        public IEnumerable<CombatSkill> CombatCards { get; }

        public int Index { get; }

        public bool IsInTankLine { get; }

        public CombatUnitState State { get; internal set; }

        public void UnscribeHandlers()
        {
            Unit.HasBeenDamaged -= Unit_HasBeenDamaged;
            Unit.HasBeenHealed -= Unit_BeenHealed;
            Unit.HasAvoidedDamage -= Unit_HasAvoidedDamage;
        }

        public void ChangeState(CombatUnitState targetState)
        {
            State = targetState;
        }

        private void Unit_BeenHealed(object? sender, int e)
        {
            HasBeenHealed?.Invoke(this, new UnitHitPointsChangedEventArgs { CombatUnit = this, Amount = e });
        }

        private void Unit_HasAvoidedDamage(object? sender, EventArgs e)
        {
            HasAvoidedDamage?.Invoke(this, EventArgs.Empty);
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

        public Unit Unit { get; }

        public event EventHandler<UnitHitPointsChangedEventArgs>? HasTakenDamage;

        internal event EventHandler<UnitHitPointsChangedEventArgs>? HasBeenHealed;

        internal event EventHandler? HasAvoidedDamage;
    }
}