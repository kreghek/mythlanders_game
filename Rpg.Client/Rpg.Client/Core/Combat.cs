using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Rpg.Client.Core.Modifiers;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Core
{
    internal sealed class Combat : ICombat
    {
        private readonly IList<CombatUnit> _allUnitList;
        private readonly Group _playerGroup;
        private readonly IList<CombatUnit> _unitQueue;
        private CombatUnit? _currentUnit;

        private int _round;

        public Combat(Group playerGroup, GlobeNode node, CombatSource combat, IDice dice, bool isAutoplay)
        {
            _playerGroup = playerGroup;
            Node = node;
            CombatSource = combat;
            Dice = dice;
            IsAutoplay = isAutoplay;
            _unitQueue = new List<CombatUnit>();
            _allUnitList = new List<CombatUnit>();
            EffectProcessor = new EffectProcessor(this, dice);
            ModifiersProcessor = new ModifiersProcessor();
        }

        public CombatUnit? CurrentUnit
        {
            get => _currentUnit;
            private set
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
                ActiveCombatUnitChanged?.Invoke(this, new UnitChangedEventArgs { NewUnit = value, OldUnit = oldUnit });

                if (_currentUnit.Unit.IsDead)
                {
                    Update();
                }
                else
                {
                    if (!IsCurrentStepCompleted)
                    {
                        CombatUnitIsReadyToControl?.Invoke(this, _currentUnit);
                    }
                    else
                    {
                        Update();
                    }
                }
            }
        }

        public bool IsAutoplay { get; }

        public GlobeNode Node { get; }

        public IEnumerable<CombatUnit> Units => _allUnitList.ToArray();

        internal CombatSource CombatSource { get; }

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

        private bool IsCurrentStepCompleted { get; set; }

        public void Surrender()
        {
            var playerCombatUnits = AliveUnits.Where(x => x.Unit.IsPlayerControlled).ToArray();
            foreach (var combatUnit in playerCombatUnits)
            {
                combatUnit.Unit.TakeDamage(playerCombatUnits.First(), 10000);
                Update();
            }

            IsCurrentStepCompleted = true;
        }

        public void UseSkill(CombatSkill skill, ICombatUnit targetUnit)
        {
            if (IsCurrentStepCompleted)
            {
                return;
            }

            if (CurrentUnit is null)
            {
                Debug.Fail("CurrentUnit is required to be assigned.");
            }

            var interactionList = new List<SkillEffectExecutionItem>();
            for (var ruleIndex = 0; ruleIndex < skill.Skill.Rules.Count; ruleIndex++)
            {
                var localRuleIndex = ruleIndex;
                var effectRule = skill.Skill.Rules[localRuleIndex];
                
                Action<ICombatUnit> ruleAction = materializedTarget =>
                {
                    EffectProcessor.Impose(new[] { effectRule }, CurrentUnit, materializedTarget);
                };

                var item = new SkillEffectExecutionItem
                {
                    Action = ruleAction,
                    Targets = EffectProcessor.GetTargets(effectRule, CurrentUnit, targetUnit)
                };
                
                interactionList.Add(item);
            }

            Action completeSkillAction = () =>
            {
                CurrentUnit.EnergyPool -= skill.EnergyCost;

                CompleteStep();
            };

            var skillExecution = new SkillExecution
            {
                SkillComplete = completeSkillAction,
                SkillRuleInteractions = interactionList
            };

            var actionEventArgs = new ActionEventArgs
            (
                skillExecution,
                CurrentUnit,
                skill.Skill,
                targetUnit
            );

            ActionGenerated?.Invoke(this, actionEventArgs);
        }

        internal void Initialize()
        {
            _allUnitList.Clear();

            foreach (var slot in _playerGroup.Slots)
            {
                if (slot.Unit is null)
                {
                    continue;
                }

                var unit = slot.Unit;
                var isAvailable = CheckUnitIsAvailable(slot.Unit);

                if (!isAvailable)
                {
                    continue;
                }

                var combatUnit = new CombatUnit(unit, slot);
                _allUnitList.Add(combatUnit);
                CombatUnitEntered?.Invoke(this, combatUnit);
            }

            foreach (var slot in CombatSource.EnemyGroup.Slots)
            {
                if (slot.Unit is null)
                {
                    continue;
                }

                var unit = slot.Unit;

                // Monster has no dead ones on start of the combat.

                var combatUnit = new CombatUnit(unit, slot);
                _allUnitList.Add(combatUnit);
                CombatUnitEntered?.Invoke(this, combatUnit);
            }

            foreach (var combatUnit in _allUnitList)
            {
                combatUnit.Unit.Dead += Unit_Dead;
                combatUnit.HasTakenHitPointsDamage += CombatUnit_HasTakenDamage;
                combatUnit.HasTakenShieldPointsDamage += CombatUnit_HasTakenDamage;
                combatUnit.IsWaiting = true;

                if (!combatUnit.Unit.IsPlayerControlled)
                {
                    AssignCpuTarget(combatUnit, Dice);
                }
            }

            ActiveCombatUnitChanged += Combat_ActiveCombatUnitChanged;
            CombatUnitIsReadyToControl += Combat_CombatUnitReadyIsToControl;

            IsCurrentStepCompleted = true;

            Update();

            AssignCpuTargetUnits();
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
                var allMonstersAreDead = monsterUnits.All(x => x.Unit.IsDead);
                var eventArgs = new CombatFinishEventArgs { Victory = anyPlayerUnitsAlive && allMonstersAreDead };
                Finish?.Invoke(this, eventArgs);
                return;
            }

            if (!NextUnit())
            {
                StartRound();
                NextRoundStarted?.Invoke(this, EventArgs.Empty);
            }

            IsCurrentStepCompleted = false;

            var currentUnit = _unitQueue.FirstOrDefault(x => !x.Unit.IsDead);
            CurrentUnit = currentUnit;
        }

        private void Ai()
        {
            var dice = GetDice();

            if (CurrentUnit is null)
            {
                return;
            }

            if (CurrentUnit.Target is not null && CurrentUnit.Target.Unit.IsDead)
            {
                CurrentUnit.Target = null;
                CurrentUnit.TargetSkill = null;
            }

            var skillsOpenList = CurrentUnit.Unit.Skills.Where(x => x.BaseEnergyCost is null).ToList();
            while (skillsOpenList.Any())
            {
                if (CurrentUnit.Target is null || CurrentUnit.TargetSkill is null)
                {
                    var skill = dice.RollFromList(skillsOpenList, 1).Single();
                    skillsOpenList.Remove(skill);

                    var possibleTargetList = GetAvailableTargets(skill, CurrentUnit);

                    if (!possibleTargetList.Any())
                    {
                        continue;
                        // There are no targets. Try another skill.
                    }

                    var targetUnit = dice.RollFromList(possibleTargetList);

                    var combatSkill = new CombatSkill(skill, new CombatSkillContext(CurrentUnit));

                    UseSkill(combatSkill, targetUnit);
                }
                else
                {
                    UseSkill(CurrentUnit.TargetSkill, CurrentUnit.Target);
                }

                return;
            }

            // No skill was used.
            Debug.Fail("Required at least one skill was used.");
        }

        private void AssignCpuTarget(CombatUnit unit, IDice dice)
        {
            var skillsOpenList = unit.CombatCards.ToList();
            while (skillsOpenList.Any())
            {
                var skill = dice.RollFromList(skillsOpenList, 1).Single();
                skillsOpenList.Remove(skill);

                var possibleTargetList = GetAvailableTargets(skill.Skill, unit);

                if (!possibleTargetList.Any())
                {
                    continue;
                    // There are no targets. Try another skill.
                }

                var targetUnit = dice.RollFromList(possibleTargetList);
                unit.Target = targetUnit;

                unit.TargetSkill = skill;
            }
        }

        private void AssignCpuTargetUnits()
        {
            var dice = GetDice();
            foreach (var cpuUnit in _unitQueue.Where(x => !x.Unit.IsPlayerControlled).ToArray())
            {
                //AssignCpuTarget(cpuUnit, dice);
            }
        }

        private static bool CheckUnitIsAvailable(Unit unit)
        {
            // Some of the player persons can be killed in previous combat in a combat sequence.
            if (unit.IsDead)
            {
                return false;
            }

            foreach (var effect in unit.GlobalEffects)
            {
                if (UnsortedHelpers.CheckIsDisabled(unit.UnitScheme.Name, effect))
                {
                    return false;
                }
            }

            return true;
        }

        private void Combat_ActiveCombatUnitChanged(object? sender, UnitChangedEventArgs e)
        {
            var oldCombatUnit = e.OldUnit;
            if (oldCombatUnit is not null)
            {
                if (!oldCombatUnit.Unit.IsDead && !oldCombatUnit.Unit.IsPlayerControlled)
                {
                    AssignCpuTarget((CombatUnit)oldCombatUnit, Dice);
                }
            }

            var combatUnit = e.NewUnit;
            if (combatUnit is null)
            {
                return;
            }

            EffectProcessor.Influence(combatUnit);

            combatUnit.Unit.RestoreShields();
            ((CombatUnit)combatUnit).IsWaiting = false;
        }

        private void Combat_CombatUnitReadyIsToControl(object? sender, CombatUnit e)
        {
            if (!e.Unit.IsPlayerControlled || IsAutoplay)
            {
                Ai();
            }
        }

        private void CombatUnit_HasTakenDamage(object? sender, UnitHitPointsChangedEventArgs e)
        {
            UnitHasBeenDamaged?.Invoke(this, e.CombatUnit);
        }

        private void CompleteStep()
        {
            IsCurrentStepCompleted = true;
        }

        private IReadOnlyList<CombatUnit> GetAvailableTargets(ISkill skill, CombatUnit unit)
        {
            switch (skill.TargetType)
            {
                case SkillTargetType.Enemy:
                    {
                        if (skill.Type == SkillType.Melee)
                        {
                            var unitsInTankPosition = Units.Where(x =>
                                    unit.Unit.IsPlayerControlled != x.Unit.IsPlayerControlled &&
                                    !x.Unit.IsDead && x.IsInTankLine)
                                .ToList();

                            if (unitsInTankPosition.Any())
                            {
                                return unitsInTankPosition;
                            }

                            return Units.Where(x =>
                                    unit.Unit.IsPlayerControlled != x.Unit.IsPlayerControlled && !x.Unit.IsDead)
                                .ToList();
                        }

                        return Units.Where(x =>
                                unit.Unit.IsPlayerControlled != x.Unit.IsPlayerControlled && !x.Unit.IsDead)
                            .ToList();
                    }

                case SkillTargetType.Friendly:
                    {
                        return Units.Where(x =>
                                unit.Unit.IsPlayerControlled == x.Unit.IsPlayerControlled && !x.Unit.IsDead)
                            .ToList();
                    }

                case SkillTargetType.Self:
                    return new[] { unit };

                default:
                    // There is a skill with unknown target. So we can't form the target list.
                    Debug.Fail("Unknown case.");

                    return Array.Empty<CombatUnit>();
            }
        }

        private IDice GetDice()
        {
            return Dice;
        }

        private void MakeUnitRoundQueue()
        {
            _unitQueue.Clear();

            var orderedByResolve = _allUnitList.OrderByDescending(x => x.Unit.UnitScheme.Resolve)
                .ThenByDescending(x => x.Unit.IsPlayerControlled).ToArray();

            foreach (var unit in orderedByResolve)
            {
                if (!unit.Unit.IsDead)
                {
                    _unitQueue.Add(unit);
                }

                unit.IsWaiting = true;
            }
        }


        private bool NextUnit()
        {
            if (_round <= 0)
            {
                return false;
            }

            // Check last unit dead yet by periodic effect.
            if (_unitQueue.Any())
            {
                _unitQueue.RemoveAt(0);
            }

            return _unitQueue.Any();
        }

        private void StartRound()
        {
            MakeUnitRoundQueue();

            AssignCpuTargetUnits();

            _round++;
        }

        private void Unit_Dead(object? sender, UnitDamagedEventArgs e)
        {
            if (sender is not Unit unit)
            {
                return;
            }

            if (e.DamageDealer.Unit.IsPlayerControlled)
            {
                var playerUnits = _allUnitList.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToArray();

                foreach (var unitToRestoreMana in playerUnits)
                {
                    unitToRestoreMana.RestoreEnergyPoint();
                }
            }

            var combatUnitInQueue = _unitQueue.FirstOrDefault(x => x.Unit == unit);
            if (combatUnitInQueue is not null)
            {
                _unitQueue.Remove(combatUnitInQueue);
            }

            unit.Dead -= Unit_Dead;

            var combatUnit = _allUnitList.FirstOrDefault(x => x.Unit == unit);

            if (combatUnit is not null)
            {
                _allUnitList.Remove(combatUnit);
                UnitDied?.Invoke(this, combatUnit);
                CombatUnitRemoved?.Invoke(this, combatUnit);
            }

            if (unit == CurrentUnit?.Unit)
            {
                CompleteStep();
            }
        }

        public IEnumerable<ICombatUnit> AliveUnits => Units.Where(x => !x.Unit.IsDead);

        public IDice Dice { get; }

        public EffectProcessor EffectProcessor { get; }

        public ModifiersProcessor ModifiersProcessor { get; }

        public void Pass()
        {
            if (CurrentUnit is null)
            {
                Debug.Fail("Current unit must be assigned");
            }

            UnitPassedTurn?.Invoke(this, CurrentUnit);
            CompleteStep();
        }

        public event EventHandler? NextRoundStarted;

        internal event EventHandler<CombatUnit>? CombatUnitRemoved;

        internal event EventHandler<UnitChangedEventArgs>? ActiveCombatUnitChanged;

        internal event EventHandler<CombatFinishEventArgs>? Finish;

        internal event EventHandler<CombatUnit>? CombatUnitEntered;

        internal event EventHandler<CombatUnit>? UnitDied;

        internal event EventHandler<CombatUnit>? UnitHasBeenDamaged;

        internal event EventHandler<CombatUnit>? UnitPassedTurn;

        internal event EventHandler<CombatUnit>? CombatUnitIsReadyToControl;

        /// <summary>
        /// Event bus for combat object interactions.
        /// </summary>
        internal event EventHandler<ActionEventArgs>? ActionGenerated;
    }
}