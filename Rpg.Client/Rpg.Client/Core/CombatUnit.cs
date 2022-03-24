using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Rpg.Client.Core.Skills;

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
            RedEnergyPool = unit.RedEnergyPoolSize;
            GreenEnergyPool = unit.GreenEnergyPoolSize;

            _skillContext = new CombatSkillContext(this);

            CombatCards = Array.Empty<CombatSkill>();

            unit.HasBeenDamaged += Unit_HasBeenDamaged;
            unit.HasBeenHealed += Unit_BeenHealed;
            unit.HasAvoidedDamage += Unit_HasAvoidedDamage;
        }

        public CombatUnit? Target { get; set; }

        public CombatSkill? TargetSkill { get; set; }

        public IReadOnlyList<CombatSkill> CombatCards { get; private set; }

        public int Index { get; }

        public bool IsInTankLine { get; }

        public int RedEnergyPool { get; set; }

        public int GreenEnergyPool { get; set; }

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

        private readonly IList<ISkill> _openSkills = new List<ISkill>();

        public void RollCombatSkills(IDice dice)
        {
            var list = new List<CombatSkill>();
            for (var i = 0; i < 4; i++)
            {
                var currentSkill = CombatCards.Any() ? CombatCards[i] : null;
                if (currentSkill is not null && currentSkill.IsLocked.GetValueOrDefault() > 0)
                {
                    currentSkill.IsLocked--;
                    list.Add(currentSkill);
                }
                else
                {
                    var rolledSkill = RollSkillFromList(dice);
                    var rolledEnv = new CombatSkillEnv
                    {
                        RedCost = dice.RollFromList(Enum.GetValues<CombatSkillCost>()),
                        GreenCost = dice.RollFromList(Enum.GetValues<CombatSkillCost>()),
                        Efficient = dice.RollFromList(Enum.GetValues<CombatSkillEfficient>()),
                        RedRegen = dice.RollFromList(Enum.GetValues<CombatSkillCost>()),
                        GreenRegen = dice.RollFromList(Enum.GetValues<CombatSkillCost>()),
                    };

                    var skillCard = new CombatSkill(rolledSkill, rolledEnv, _skillContext);
                    list.Add(skillCard);   
                }
            }

            CombatCards = list;
        }

        private ISkill RollSkillFromList(IDice dice)
        {
            if (!_openSkills.Any())
            {
                FillOpenSkillList();
            }

            var rolledSkillIndex = dice.RollArrayIndex(_openSkills);
            var rolledSkill = _openSkills[rolledSkillIndex];
            _openSkills.RemoveAt(rolledSkillIndex);
            
            return rolledSkill;
        }

        private void FillOpenSkillList()
        {
            foreach (var skill in Unit.Skills)
            {
                for (int i = 0; i < skill.Weight; i++)
                {
                    _openSkills.Add(skill);   
                }
            }
        }

        public Unit Unit { get; }

        internal event EventHandler<UnitHitPointsChangedEventArgs>? HasTakenDamage;

        internal event EventHandler<UnitHitPointsChangedEventArgs>? HasBeenHealed;

        internal event EventHandler? HasAvoidedDamage;
    }
}