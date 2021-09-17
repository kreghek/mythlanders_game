﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Rpg.Client.Core.Skills;
using Rpg.Client.Models.Biome.GameObjects;

namespace Rpg.Client.Core
{
    internal class ActiveCombat
    {
        private readonly IList<CombatUnit> _allUnitList;
        private readonly IDice _dice;
        private readonly Group _playerGroup;
        private readonly IList<CombatUnit> _unitQueue;
        private CombatUnit _currentUnit;
        public EffectProcessor EffectProcessor { get; }

        private int _round;

        public ActiveCombat(Group playerGroup, GlobeNodeGameObject node, Combat combat, Biome biom, IDice dice)
        {
            _playerGroup = playerGroup;
            Node = node;
            Combat = combat;
            Biom = biom;
            _dice = dice;
            _unitQueue = new List<CombatUnit>();
            _allUnitList = new List<CombatUnit>();
            EffectProcessor = new EffectProcessor(this, _dice);
        }

        public Biome Biom { get; }

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
        public IEnumerable<CombatUnit> AliveUnits => Units.Where(x => !x.Unit.IsDead);

        public GlobeNodeGameObject Node { get; }

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

        public void UseSkill(SkillBase skill, CombatUnit target)
        {
            Action action = () => EffectProcessor.Influence(skill.Rules, CurrentUnit, target);

            ActionGenerated?.Invoke(this, new ActionEventArgs
            {
                Action = action,
                Actor = CurrentUnit,
                Skill = skill,
                Target = target
            });
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
                var allMonstersAreDead = Units.Any(x => x.Unit.IsDead && !x.Unit.IsPlayerControlled);
                var eventArgs = new CombatFinishEventArgs { Victory = allMonstersAreDead };
                Finish?.Invoke(this, eventArgs);
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

            var targetPlayerObject =
                        dice.RollFromList(Units.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToList(), 1)
                            .Single();

            UseSkill(skill, targetPlayerObject);

            //var combatPowerScope = skill.Scope;
            ////TODO Specify combat power scope scope in the monsters.
            //if (combatPowerScope == SkillScope.Undefined)
            //{
            //    combatPowerScope = SkillScope.Single;
            //    skill.Scope = SkillScope.Single;
            //}

            //switch (skill.Type)
            //{
            //    case SkillScope.Single:
            //        var targetPlayerObject =
            //            dice.RollFromList(Units.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToList(), 1)
            //                .Single();
            //        UseSkill(skill, targetPlayerObject);
            //        break;

            //    case SkillScope.AllEnemyGroup:
            //        UseSkill(skill);
            //        break;

            //    case SkillScope.Undefined:
            //    default:
            //        Debug.Fail($"Unknown combat power scope {combatPowerScope}.");
            //        break;
            //}
        }

        private IDice GetDice()
        {
            return _dice;
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
            if (sender is not Unit unit)
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

        internal event EventHandler<SkillUsingEventArgs>? BeforeSkillUsing;

        internal event EventHandler<SkillUsingEventArgs>? AfterSkillUsing;

        internal event EventHandler<CombatUnit>? MoveCompleted;

        internal event EventHandler<CombatUnit>? UnitHadDamage;

        /// <summary>
        /// Event bus for combat object interactions.
        /// </summary>
        internal event EventHandler<ActionEventArgs>? ActionGenerated;

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
            public SkillBase Skill { get; set; }
            public CombatUnit Target { get; set; }
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