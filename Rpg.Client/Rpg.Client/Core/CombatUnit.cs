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

            unit.HasBeenDamaged += Unit_HasBeenDamaged;
            unit.HasBeenHealed += Unit_BeenHealed;
            unit.HasAvoidedDamage += Unit_HasAvoidedDamage;
            unit.SchemeAutoTransition += Unit_SchemeAutoTransition;
        }

        private void Unit_SchemeAutoTransition(object? sender, AutoTransitionEventArgs e)
        {
            Target = null;
            TargetSkill = null;

            var skillContext = new CombatSkillContext(this);
            CombatCards = CreateCombatSkills(Unit.Skills, skillContext);
        }

        private static IReadOnlyList<CombatSkill> CreateCombatSkills(IEnumerable<ISkill> unitSkills,
            ICombatSkillContext combatSkillContext)
        {
            return unitSkills.Select(skill => new CombatSkill(skill, combatSkillContext)).ToList();
        }

        public CombatUnit? Target { get; set; }

        public CombatSkill? TargetSkill { get; set; }

        public IReadOnlyList<CombatSkill> CombatCards { get; private set; }

        public int Index { get; }

        public bool IsInTankLine { get; }

        public int EnergyPool { get; set; }

        public CombatUnitState State { get; private set; }

        public void UnsubscribeHandlers()
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

        public void RestoreEnergyPoint()
        {
            EnergyPool++;
            if (EnergyPool > Unit.EnergyPoolSize)
            {
                EnergyPool = Unit.EnergyPoolSize;
            }
        }
    }
}