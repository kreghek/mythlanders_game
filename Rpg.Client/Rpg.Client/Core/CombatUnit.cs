using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class CombatUnit : ICombatUnit
    {
        private readonly CombatSkillContext _skillContext;

        public CombatUnit(Unit unit, GroupSlot slot)
        {
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            Index = slot.Index;
            IsInTankLine = slot.IsTankLine;

            _skillContext = new CombatSkillContext(this);

            CombatCards = Array.Empty<CombatSkill>();

            unit.HasBeenDamaged += Unit_HasBeenDamaged;
            unit.HasBeenHealed += Unit_BeenHealed;
            unit.HasAvoidedDamage += Unit_HasAvoidedDamage;
        }

        public IReadOnlyList<CombatSkill> CombatCards { get; private set; }

        public int Index { get; }

        public bool IsInTankLine { get; }

        public CombatUnitState State { get; private set; }

        public void UnsubscribeHandlers()
        {
            Unit.HasBeenDamaged -= Unit_HasBeenDamaged;
            Unit.HasBeenHealed -= Unit_BeenHealed;
            Unit.HasAvoidedDamage -= Unit_HasAvoidedDamage;
        }

        internal void ChangeState(CombatUnitState targetState)
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

        public void RollCombatSkills(IDice dice)
        {
            var list = new List<CombatSkill>();
            for (var i = 0; i < 4; i++)
            {
                var rolledSkill = dice.RollFromList(Unit.Skills.ToArray());
                var skillCard = new CombatSkill(rolledSkill, _skillContext);
                list.Add(skillCard);
            }

            CombatCards = list;
        }

        public Unit Unit { get; }

        internal event EventHandler<UnitHitPointsChangedEventArgs>? HasTakenDamage;

        internal event EventHandler<UnitHitPointsChangedEventArgs>? HasBeenHealed;

        internal event EventHandler? HasAvoidedDamage;
    }
}