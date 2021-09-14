using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rpg.Client.Core
{
    internal class ActiveCombat
    {
        private readonly IList<CombatUnit> _allUnitList;
        private readonly Group _playerGroup;
        private readonly IDice _dice;
        private readonly IList<CombatUnit> _unitQueue;
        private CombatUnit _currentUnit;

        private int _round;

        public ActiveCombat(Group playerGroup, Combat combat, Biom biom, IDice dice)
        {
            _playerGroup = playerGroup;
            Combat = combat;
            Biom = biom;
            _dice = dice;
            _unitQueue = new List<CombatUnit>();
            _allUnitList = new List<CombatUnit>();
        }

        public Biom Biom { get; }

        public CombatUnit? CurrentUnit
        {
            get => _currentUnit;
            set
            {
                if (_currentUnit == value)
                {
                    return;
                }

                var oldUnit = _currentUnit;

                _currentUnit = value;
                UnitChanged?.Invoke(this, new UnitChangedEventArgs { NewUnit = _currentUnit, OldUnit = oldUnit });
            }
        }

        public IEnumerable<CombatUnit> Units => _allUnitList.ToArray();

        internal Combat Combat { get; }

        internal bool Finished
        {
            get
            {
                var playerUnits = _allUnitList.Where(x => !x.Unit.IsDead && x.Unit.IsPlayerControlled);
                var hasPlayerUnits = playerUnits.Any();

                var cpuUnits = _allUnitList.Where(x => !x.Unit.IsDead && !x.Unit.IsPlayerControlled);
                var hasCpuUnits = cpuUnits.Any();

                // TODO Looks like XOR
                if (hasPlayerUnits && !hasCpuUnits)
                {
                    return true;
                }

                if (!hasPlayerUnits && hasCpuUnits)
                {
                    return true;
                }

                return false;
            }
        }

        public void UseSkill(CombatSkill skill, CombatUnit target)
        {
            if (skill.Scope != SkillScope.Single)
            {
                throw new InvalidOperationException("Не верные рамки скила");
            }

            var dice = GetDice();

            Action action;

            if (target.Unit.IsPlayerControlled == CurrentUnit.Unit.IsPlayerControlled)
            {
                if (skill.TargetType != SkillTarget.Friendly)
                {
                    throw new InvalidOperationException("Не верная цель скила");
                }

                action = () =>
                {
                    BeforeSkillUsing?.Invoke(this,
                        new SkillUsingEventArgs { Actor = CurrentUnit, Skill = skill, Target = target });
                    target.Unit.TakeHeal(dice.Roll(skill.DamageMin, skill.DamageMax));
                    AfterSkillUsing?.Invoke(this,
                        new SkillUsingEventArgs { Actor = CurrentUnit, Skill = skill, Target = target });
                    MoveCompleted?.Invoke(this, CurrentUnit);
                };
            }
            else
            {
                if (skill.TargetType != SkillTarget.Enemy)
                {
                    throw new InvalidOperationException("Не верная цель скила");
                }

                action = () =>
                {
                    BeforeSkillUsing?.Invoke(this,
                        new SkillUsingEventArgs { Actor = CurrentUnit, Skill = skill, Target = target });
                    target.Unit.TakeDamage(dice.Roll(skill.DamageMin, skill.DamageMax));
                    AfterSkillUsing?.Invoke(this,
                        new SkillUsingEventArgs { Actor = CurrentUnit, Skill = skill, Target = target });
                    MoveCompleted?.Invoke(this, CurrentUnit);
                };
            }

            ActionGenerated?.Invoke(this, new ActionEventArgs
            {
                Action = action,
                Actor = CurrentUnit,
                Skill = skill,
                Target = target
            });
        }

        public void UseSkill(CombatSkill skill)
        {
            if (skill.Scope != SkillScope.AllEnemyGroup)
            {
                throw new InvalidOperationException("Не верные рамки скила");
            }

            var dice = GetDice();

            var unitsGroup = CurrentUnit.Unit.IsPlayerControlled
                ? _allUnitList.Where(x => !x.Unit.IsPlayerControlled && !x.Unit.IsDead)
                : _allUnitList.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead);

            // Mass skill
            Action action = () =>
            {
                BeforeSkillUsing?.Invoke(this, new SkillUsingEventArgs { Actor = CurrentUnit, Skill = skill });
                foreach (var unit in unitsGroup)
                {
                    unit.Unit.TakeDamage(dice.Roll(skill.DamageMin, skill.DamageMax));
                }

                AfterSkillUsing?.Invoke(this, new SkillUsingEventArgs { Actor = CurrentUnit, Skill = skill });
                MoveCompleted?.Invoke(this, CurrentUnit);
            };

            ActionGenerated?.Invoke(this, new ActionEventArgs
            {
                Action = action,
                Actor = CurrentUnit,
                Skill = skill
            });
        }

        private IDice GetDice()
        {
            return _dice;
        }
        internal void Initialize()
        {
            _allUnitList.Clear();

            var index = 0;
            foreach (var unit in _playerGroup.Units)
            {
                var combatUnit = new CombatUnit(unit, index);
                _allUnitList.Add(combatUnit);
                UnitEntered?.Invoke(this, combatUnit);
                index++;
            }

            index = 0;
            foreach (var unit in Combat.EnemyGroup.Units)
            {
                var combatUnit = new CombatUnit(unit, index);
                _allUnitList.Add(combatUnit);
                UnitEntered?.Invoke(this, combatUnit);
                index++;
            }

            foreach (var unit in _allUnitList)
            {
                unit.Unit.Dead += Unit_Dead;
            }

            UnitChanged += ActiveCombat_UnitChanged;
        }

        internal void Update()
        {
            if (Finished)
            {
                Finish?.Invoke(this,
                    new CombatFinishEventArgs
                        { Victory = Units.Any(x => x.Unit.IsDead && !x.Unit.IsPlayerControlled) });
                return;
            }

            if (!NextUnit())
            {
                StartRound();
            }

            CurrentUnit = _unitQueue.FirstOrDefault(x => !x.Unit.IsDead);
        }

        private void ActiveCombat_UnitChanged(object? sender, UnitChangedEventArgs e)
        {
            if (e.NewUnit?.Unit.IsPlayerControlled == false)
            {
                AI();
            }
        }

        private void AI()
        {
            var dice = GetDice();

            if (CurrentUnit is null)
            {
                return;
            }

            var skills = CurrentUnit.Unit.Skills.ToArray();
            var skill = dice.RollFromList(skills, 1).Single();

            var combatPowerScope = skill.Scope;
            //TODO Specify combat power scope scope in the monsters.
            if (combatPowerScope == SkillScope.Undefined)
            {
                combatPowerScope = SkillScope.Single;
                skill.Scope = SkillScope.Single;
            }

            switch (combatPowerScope)
            {
                case SkillScope.Single:
                    var targetPlayerObject =
                        dice.RollFromList(Units.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToList(), 1)
                            .Single();
                    UseSkill(skill, targetPlayerObject);
                    break;

                case SkillScope.AllEnemyGroup:
                    UseSkill(skill);
                    break;

                case SkillScope.Undefined:
                default:
                    Debug.Fail($"Unknown combat power scope {combatPowerScope}.");
                    break;
            }
        }


        private bool NextUnit()
        {
            if (_round <= 0)
            {
                return false;
            }

            _unitQueue.RemoveAt(0);
            return _unitQueue.Count != 0;
        }

        private void StartRound()
        {
            _unitQueue.Clear();

            foreach (var unit in _allUnitList)
            {
                if (!unit.Unit.IsDead)
                {
                    _unitQueue.Add(unit);
                }
            }

            _round++;
        }

        private void Unit_Dead(object? sender, EventArgs e)
        {
            if (!(sender is Unit unit))
            {
                return;
            }

            var combatUnit = _unitQueue.First(x => x.Unit == unit);
            _unitQueue.Remove(combatUnit);

            unit.Dead -= Unit_Dead;
            UnitDied?.Invoke(this, combatUnit);
        }

        internal event EventHandler<UnitChangedEventArgs>? UnitChanged;

        internal event EventHandler<CombatFinishEventArgs>? Finish;

        internal event EventHandler<CombatUnit>? UnitEntered;

        internal event EventHandler<CombatUnit>? UnitDied;

        internal event EventHandler<SkillUsingEventArgs> BeforeSkillUsing;

        internal event EventHandler<SkillUsingEventArgs> AfterSkillUsing;

        internal event EventHandler<CombatUnit>? MoveCompleted;

        internal event EventHandler<CombatUnit>? UnitHadDamage;


        internal event EventHandler<ActionEventArgs> ActionGenerated;

        internal class SkillUsingEventArgs : EventArgs
        {
            public CombatUnit Actor { get; set; }
            public CombatSkill Skill { get; set; }
            public CombatUnit? Target { get; set; }
        }

        internal class ActionEventArgs : EventArgs
        {
            public Action Action { get; set; }
            public CombatUnit Actor { get; set; }
            public CombatSkill Skill { get; set; }
            public CombatUnit? Target { get; set; }
        }

        internal class UnitChangedEventArgs : EventArgs
        {
            public CombatUnit? NewUnit { get; set; }
            public CombatUnit? OldUnit { get; set; }
        }

        internal class CombatFinishEventArgs : EventArgs
        {
            public bool Victory { get; set; }
        }
    }
}