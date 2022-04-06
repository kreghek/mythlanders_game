using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class CombatUnit : ICombatUnit
    {
        public CombatUnit(Unit unit, GroupSlot slot)
        {
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            Index = slot.Index;
            IsInTankLine = slot.IsTankLine;
            EnergyPool = unit.EnergyPoolSize;

            var skillContext = new CombatSkillContext(this);
            CombatCards = CreateCombatSkills(unit.Skills, skillContext);

            unit.HasBeenHitPointsDamaged += Unit_HasBeenHitPointsDamaged;
            unit.HasBeenShieldPointsDamaged += Unit_HasBeenShieldPointsDamaged;
            unit.HasBeenHitPointsRestored += Unit_BeenHealed;
            unit.HasBeenShieldPointsRestored += Unit_HasBeenShieldRestored;
            unit.HasAvoidedDamage += Unit_HasAvoidedDamage;
            unit.Blocked += Unit_Blocked;
            unit.SchemeAutoTransition += Unit_SchemeAutoTransition;
        }

        private void Unit_Blocked(object? sender, EventArgs e)
        {
            Blocked?.Invoke(this, EventArgs.Empty);
        }

        private void Unit_HasBeenShieldRestored(object? sender, int e)
        {
            HasBeenShieldPointsRestored?.Invoke(this, new UnitHitPointsChangedEventArgs { CombatUnit = this, Amount = e });
        }

        private void Unit_HasBeenShieldPointsDamaged(object? sender, UnitHasBeenDamagedEventArgs e)
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
            HasTakenShieldPointsDamage?.Invoke(this, args);
        }

        public int Index { get; }

        public bool IsInTankLine { get; }

        public CombatUnitState State { get; private set; }

        public CombatUnit? Target { get; set; }

        public CombatSkill? TargetSkill { get; set; }

        public void UnsubscribeHandlers()
        {
            Unit.HasBeenHitPointsDamaged -= Unit_HasBeenHitPointsDamaged;
            Unit.HasBeenShieldPointsDamaged -= Unit_HasBeenShieldPointsDamaged;
            Unit.HasBeenHitPointsRestored -= Unit_BeenHealed;
            Unit.HasBeenShieldPointsRestored -= Unit_HasBeenShieldRestored;
            Unit.HasAvoidedDamage -= Unit_HasAvoidedDamage;
            Unit.SchemeAutoTransition -= Unit_SchemeAutoTransition;
            Unit.Blocked -= Unit_Blocked;
        }

        private static IReadOnlyList<CombatSkill> CreateCombatSkills(IEnumerable<ISkill> unitSkills,
            ICombatSkillContext combatSkillContext)
        {
            return unitSkills.Select(skill => new CombatSkill(skill, combatSkillContext)).ToList();
        }

        private void Unit_BeenHealed(object? sender, int e)
        {
            HasBeenHitPointsRestored?.Invoke(this, new UnitHitPointsChangedEventArgs { CombatUnit = this, Amount = e });
        }

        private void Unit_HasAvoidedDamage(object? sender, EventArgs e)
        {
            HasAvoidedDamage?.Invoke(this, EventArgs.Empty);
        }

        private void Unit_HasBeenHitPointsDamaged(object? sender, UnitHasBeenDamagedEventArgs e)
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
            HasTakenHitPointsDamage?.Invoke(this, args);
        }

        private void Unit_SchemeAutoTransition(object? sender, AutoTransitionEventArgs e)
        {
            Target = null;
            TargetSkill = null;

            var skillContext = new CombatSkillContext(this);
            CombatCards = CreateCombatSkills(Unit.Skills, skillContext);
        }

        public IReadOnlyList<CombatSkill> CombatCards { get; private set; }

        public int EnergyPool { get; set; }

        public void ChangeState(CombatUnitState targetState)
        {
            State = targetState;
        }

        public Unit Unit { get; }

        public event EventHandler<UnitHitPointsChangedEventArgs>? HasTakenHitPointsDamage;
        
        public event EventHandler<UnitHitPointsChangedEventArgs>? HasTakenShieldPointsDamage;

        public event EventHandler? Blocked;

        public void RestoreEnergyPoint()
        {
            EnergyPool++;
            if (EnergyPool > Unit.EnergyPoolSize)
            {
                EnergyPool = Unit.EnergyPoolSize;
            }
        }

        internal event EventHandler<UnitHitPointsChangedEventArgs>? HasBeenHitPointsRestored;
        
        internal event EventHandler<UnitHitPointsChangedEventArgs>? HasBeenShieldPointsRestored;

        internal event EventHandler? HasAvoidedDamage;
    }
}