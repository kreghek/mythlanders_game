using System;
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

        public ActiveCombat(Group playerGroup, GlobeNodeGameObject node, Combat combat, Biome biom, IDice dice)
        {
            _playerGroup = playerGroup;
            Node = node;
            Combat = combat;
            Biom = biom;
            Dice = dice;
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
                    UnitReadyToControl?.Invoke(this, _currentUnit);
                }
                else
                {
                    Update();
                }
            }
        }

        public IDice Dice { get; }

        public EffectProcessor EffectProcessor { get; }

        public bool IsCurrentStepCompleted { get; set; }

        public ModifiersProcessor ModifiersProcessor { get; }

        public GlobeNodeGameObject Node { get; }

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

        public void UseSkill(SkillBase skill, CombatUnit target)
        {
            if (IsCurrentStepCompleted)
            {
                return;
            }

            if (skill.Cost is not null)
            {
                CurrentUnit.Unit.ManaPool -= skill.Cost.Value;
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
            UnitReadyToControl += ActiveCombat_UnitReadyToControl;

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
                var allMonstersAreDead = Units.Any(x => x.Unit.IsDead && !x.Unit.IsPlayerControlled);
                var eventArgs = new CombatFinishEventArgs { Victory = allMonstersAreDead };
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
            if (!e.Unit.IsPlayerControlled)
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

            IList<CombatUnit> possibleTargetList;
            switch (skill.TargetType)
            {
                case SkillTargetType.Enemy:
                    {
                        possibleTargetList = Units.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToList();
                        break;
                    }

                case SkillTargetType.Friendly:
                    {
                        possibleTargetList = Units.Where(x => !x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToList();
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
                e.Damager.Unit.RestoreManaPoint();
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

        internal event EventHandler<UnitChangedEventArgs>? UnitChanged;

        internal event EventHandler<CombatFinishEventArgs>? Finish;

        internal event EventHandler<CombatUnit>? UnitEntered;

        internal event EventHandler<CombatUnit>? UnitDied;

        internal event EventHandler<SkillUsingEventArgs>? BeforeSkillUsing;

        internal event EventHandler<SkillUsingEventArgs>? AfterSkillUsing;

        internal event EventHandler<CombatUnit>? MoveCompleted;

        internal event EventHandler<CombatUnit>? UnitHadDamage;

        internal event EventHandler<CombatUnit>? UnitPassed;

        internal event EventHandler<CombatUnit>? UnitReadyToControl;

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