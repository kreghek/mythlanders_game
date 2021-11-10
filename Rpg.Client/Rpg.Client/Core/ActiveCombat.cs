﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Rpg.Client.Core.Effects;
using Rpg.Client.Core.Modifiers;
using Rpg.Client.Core.Skills;
using Rpg.Client.Models.Biome.GameObjects;

namespace Rpg.Client.Core
{
    internal class ActiveCombat
    {
        private readonly IList<CombatUnit> _allUnitList;
        private readonly Group _playerGroup;
        private readonly IList<CombatUnit> _unitQueue;
        private CombatUnit? _currentUnit;

        private int _round;

        public ActiveCombat(Group playerGroup, GlobeNode node, Combat combat, Biome biom, IDice dice,
            bool isAutoplay)
        {
            _playerGroup = playerGroup;
            Node = node;
            Combat = combat;
            Biom = biom;
            Dice = dice;
            IsAutoplay = isAutoplay;
            _unitQueue = new List<CombatUnit>();
            _allUnitList = new List<CombatUnit>();
            EffectProcessor = new EffectProcessor(this);
            ModifiersProcessor = new ModifiersProcessor();
        }

        public IEnumerable<CombatUnit> AliveUnits => Units.Where(x => !x.Unit.IsDead);

        public Biome Biom { get; }

        public CombatUnit? CurrentUnit
        {
            get => _currentUnit;
            set
            {
                if (value is null)
                {
                    return;
                }

                if (_currentUnit == value)
                {
                    return;
                }

                var oldUnit = _currentUnit;

                _currentUnit = value;
                UnitChanged?.Invoke(this, new UnitChangedEventArgs { NewUnit = value, OldUnit = oldUnit });

                if (!IsCurrentStepCompleted)
                {
                    CombatUnitReadyToControl?.Invoke(this, _currentUnit);
                }
                else
                {
                    Update();
                }
            }
        }

        public IDice Dice { get; }

        public EffectProcessor EffectProcessor { get; }

        public bool IsAutoplay { get; }

        public bool IsCurrentStepCompleted { get; set; }

        public ModifiersProcessor ModifiersProcessor { get; }

        public GlobeNode Node { get; }

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

        public void Pass()
        {
            UnitPassed?.Invoke(this, CurrentUnit);
            CompleteStep();
        }

        public void UseSkill(ISkill skill, CombatUnit target)
        {
            if (IsCurrentStepCompleted)
            {
                return;
            }

            if (skill.ManaCost is not null)
            {
                CurrentUnit.Unit.ManaPool -= skill.ManaCost.Value;
            }

            Action action = () =>
            {
                EffectProcessor.Impose(skill.Rules, CurrentUnit, target);
                CompleteStep();
            };

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
                // Some of the player persons can be killed in previos combat in a combat sequence.

                if (!unit.IsDead)
                {
                    var combatUnit = new CombatUnit(unit, index);
                    _allUnitList.Add(combatUnit);
                    CombatUnitEntered?.Invoke(this, combatUnit);
                }

                index++;
            }

            index = 0;
            foreach (var unit in Combat.EnemyGroup.Units)
            {
                // Monster has no deads on start of the combat.

                var combatUnit = new CombatUnit(unit, index);
                _allUnitList.Add(combatUnit);
                CombatUnitEntered?.Invoke(this, combatUnit);
                index++;
            }

            foreach (var combatUnit in _allUnitList)
            {
                combatUnit.Unit.Dead += Unit_Dead;
                combatUnit.HasTakenDamage += CombatUnit_HasTakenDamage;
            }

            UnitChanged += ActiveCombat_UnitChanged;
            CombatUnitReadyToControl += ActiveCombat_UnitReadyToControl;

            IsCurrentStepCompleted = true;
        }

        internal void Update()
        {
            if (!IsCurrentStepCompleted)
            {
                return;
            }

            if (Finished)
            {
                var playerUnits = Units.Where(x => x.Unit.IsPlayerControlled).ToArray();
                var monsterUnits = Units.Where(x => !x.Unit.IsPlayerControlled).ToArray();

                var anyPlayerUnitsAlive = playerUnits.Any(x => !x.Unit.IsDead);
                var allMonstersAreDead = !monsterUnits.Any(x => !x.Unit.IsDead);
                var eventArgs = new CombatFinishEventArgs { Victory = anyPlayerUnitsAlive && allMonstersAreDead };
                Finish?.Invoke(this, eventArgs);
                return;
            }

            if (!NextUnit())
            {
                StartRound();
            }

            IsCurrentStepCompleted = false;

            CurrentUnit = _unitQueue.FirstOrDefault(x => !x.Unit.IsDead);
        }

        private void ActiveCombat_UnitChanged(object? sender, UnitChangedEventArgs e)
        {
            if (e.NewUnit is null)
            {
                return;
            }

            EffectProcessor.Influence(e.NewUnit);
        }

        private void ActiveCombat_UnitReadyToControl(object? sender, CombatUnit e)
        {
            if (!e.Unit.IsPlayerControlled || IsAutoplay)
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

            var skills = CurrentUnit.Unit.Skills.Where(x => x.ManaCost is null).ToArray();
            var skill = dice.RollFromList(skills, 1).Single();

            IList<CombatUnit> possibleTargetList;
            switch (skill.TargetType)
            {
                case SkillTargetType.Enemy:
                    {
                        possibleTargetList = Units.Where(x =>
                                CurrentUnit.Unit.IsPlayerControlled != x.Unit.IsPlayerControlled && !x.Unit.IsDead)
                            .ToList();
                        break;
                    }

                case SkillTargetType.Friendly:
                    {
                        possibleTargetList = Units.Where(x =>
                                CurrentUnit.Unit.IsPlayerControlled == x.Unit.IsPlayerControlled && !x.Unit.IsDead)
                            .ToList();
                        break;
                    }

                default:
                    // There is a skill with unknown target. So we can't form the target list.
                    Debug.Fail("Unknown case.");
                    return;
            }

            var targetPlayerObject = dice.RollFromList(possibleTargetList);

            UseSkill(skill, targetPlayerObject);
        }

        private void CombatUnit_HasTakenDamage(object? sender, CombatUnit.UnitHpChangedEventArgs e)
        {
            var unit = e.CombatUnit.Unit;
            var transition = unit.UnitScheme.SchemeAudoTransiton;
            if (transition is not null)
            {
                var currentHpShare = (float)unit.Hp / unit.MaxHp;
                if (currentHpShare <= transition.HpShare)
                {
                    unit.Hp = (int)(transition.HpShare * unit.MaxHp);
                    var nextScheme = transition.NextScheme;

                    var combatUnit = e.CombatUnit;
                    ReplaceUnitToNewForm(nextScheme, combatUnit);
                }
            }

            if (unit.IsDead)
            {
                UnitDied?.Invoke(this, e.CombatUnit);
            }
            else
            {
                UnitHasBeenDamaged?.Invoke(this, e.CombatUnit);
            }
        }

        private void CompleteStep()
        {
            IsCurrentStepCompleted = true;
        }

        private IDice GetDice()
        {
            return Dice;
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

        private void ReplaceUnitToNewForm(UnitScheme nextScheme, CombatUnit combatUnit)
        {
            _allUnitList.Remove(combatUnit);
            _unitQueue.Remove(combatUnit);

            combatUnit.Unit.Dead -= Unit_Dead;
            combatUnit.HasTakenDamage -= CombatUnit_HasTakenDamage;

            var newFormUnit = new Unit(nextScheme, combatUnit.Unit.Level)
            {
                Hp = combatUnit.Unit.Hp
            };
            var newFormCombatUnit = new CombatUnit(newFormUnit, combatUnit.Index);

            newFormCombatUnit.Unit.Dead += Unit_Dead;
            newFormCombatUnit.HasTakenDamage += CombatUnit_HasTakenDamage;

            CombatUnitRemoved?.Invoke(this, combatUnit);
            _allUnitList.Add(newFormCombatUnit);
            CombatUnitEntered?.Invoke(this, newFormCombatUnit);
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

        private void Unit_Dead(object? sender, Unit.UnitDamagedEventArgs e)
        {
            if (sender is not Unit unit)
            {
                return;
            }

            if (e.Damager.Unit.IsPlayerControlled)
            {
                var playerUnits = _allUnitList.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToArray();

                foreach (var unitToRestoreMana in playerUnits)
                {
                    unitToRestoreMana.Unit.RestoreManaPoint();
                }
            }

            var combatUnit = _unitQueue.FirstOrDefault(x => x.Unit == unit);
            if (combatUnit is not null)
            {
                _unitQueue.Remove(combatUnit);
            }

            unit.Dead -= Unit_Dead;

            if (combatUnit is not null)
            {
                UnitDied?.Invoke(this, combatUnit);
            }
        }

        public event EventHandler<CombatUnit> CombatUnitRemoved;

        internal event EventHandler<UnitChangedEventArgs>? UnitChanged;

        internal event EventHandler<CombatFinishEventArgs>? Finish;

        internal event EventHandler<CombatUnit>? CombatUnitEntered;

        internal event EventHandler<CombatUnit>? UnitDied;

        internal event EventHandler<CombatUnit>? UnitHasBeenDamaged;

        internal event EventHandler<CombatUnit>? UnitPassed;

        internal event EventHandler<CombatUnit>? CombatUnitReadyToControl;

        /// <summary>
        /// Event bus for combat object interactions.
        /// </summary>
        internal event EventHandler<ActionEventArgs>? ActionGenerated;

        internal class ActionEventArgs : EventArgs
        {
            public Action Action { get; set; }
            public CombatUnit Actor { get; set; }
            public ISkill Skill { get; set; }
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