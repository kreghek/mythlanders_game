using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Rpg.Client.Core.Modifiers;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal class Combat
    {
        private readonly IList<CombatUnit> _allUnitList;
        private readonly Group _playerGroup;
        private readonly IList<CombatUnit> _unitQueue;
        private CombatUnit? _currentUnit;

        private int _round;

        public Combat(Group playerGroup, GlobeNode node, CombatSource combat, Biome biome, IDice dice,
            bool isAutoplay)
        {
            _playerGroup = playerGroup;
            Node = node;
            CombatSource = combat;
            Biome = biome;
            Dice = dice;
            IsAutoplay = isAutoplay;
            _unitQueue = new List<CombatUnit>();
            _allUnitList = new List<CombatUnit>();
            EffectProcessor = new EffectProcessor(this);
            ModifiersProcessor = new ModifiersProcessor();
        }

        public IEnumerable<CombatUnit> AliveUnits => Units.Where(x => !x.Unit.IsDead);

        public Biome Biome { get; }

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

        public IDice Dice { get; }

        public EffectProcessor EffectProcessor { get; }

        public bool IsAutoplay { get; }

        public ModifiersProcessor ModifiersProcessor { get; }

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

        public void Pass()
        {
            if (CurrentUnit is null)
            {
                Debug.Fail("Current unit must be assigned");
            }

            UnitPassedTurn?.Invoke(this, CurrentUnit);
            CompleteStep();
        }

        public void UseSkill(ISkill skill, CombatUnit targetUnit)
        {
            if (IsCurrentStepCompleted)
            {
                return;
            }

            if (CurrentUnit is null)
            {
                Debug.Fail("CurrentUnit is required to be assigned.");
            }

            if (skill.ManaCost is not null)
            {
                CurrentUnit.Unit.ManaPool -= skill.ManaCost.Value;
            }

            Action action = () =>
            {
                EffectProcessor.Impose(skill.Rules, CurrentUnit, targetUnit);
                CompleteStep();
            };

            var actionEventArgs = new ActionEventArgs
            (
                action,
                CurrentUnit,
                skill,
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
                combatUnit.HasTakenDamage += CombatUnit_HasTakenDamage;
            }

            ActiveCombatUnitChanged += Combat_ActiveCombatUnitChanged;
            CombatUnitIsReadyToControl += Combat_CombatUnitReadyIsToControl;

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
                var allMonstersAreDead = monsterUnits.All(x => x.Unit.IsDead);
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

        private void Ai()
        {
            var dice = GetDice();

            if (CurrentUnit is null)
            {
                return;
            }

            var skillsOpenList = CurrentUnit.Unit.Skills.Where(x => x.ManaCost is null).ToList();
            while (skillsOpenList.Any())
            {
                var skill = dice.RollFromList(skillsOpenList, 1).Single();
                skillsOpenList.Remove(skill);

                var possibleTargetList = GetAvailableTargets(skill);

                if (!possibleTargetList.Any())
                {
                    continue;
                    // There are no targets. Try another skill.
                }

                var targetPlayerObject = dice.RollFromList(possibleTargetList);

                UseSkill(skill, targetPlayerObject);

                return;
            }

            // No skill was used.
            Debug.Fail("Required at least one skill was used.");
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
                if (unit.UnitScheme.Name == UnitName.Berimir && effect.Source.IsActive &&
                    effect.Source.GetRules().Contains(GlobeRule.DisableBerimir))
                {
                    return false;
                }
            }

            return true;
        }

        private void Combat_ActiveCombatUnitChanged(object? sender, UnitChangedEventArgs e)
        {
            if (e.NewUnit is null)
            {
                return;
            }

            EffectProcessor.Influence(e.NewUnit);
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

        private IReadOnlyList<CombatUnit> GetAvailableTargets(ISkill skill)
        {
            switch (skill.TargetType)
            {
                case SkillTargetType.Enemy:
                    {
                        if (skill.Type == SkillType.Melee)
                        {
                            var unitsInTankPosition = Units.Where(x =>
                                    CurrentUnit.Unit.IsPlayerControlled != x.Unit.IsPlayerControlled &&
                                    !x.Unit.IsDead && x.IsInTankLine)
                                .ToList();

                            if (unitsInTankPosition.Any())
                            {
                                return unitsInTankPosition;
                            }

                            return Units.Where(x =>
                                    CurrentUnit.Unit.IsPlayerControlled != x.Unit.IsPlayerControlled && !x.Unit.IsDead)
                                .ToList();
                        }

                        return Units.Where(x =>
                                CurrentUnit.Unit.IsPlayerControlled != x.Unit.IsPlayerControlled && !x.Unit.IsDead)
                            .ToList();
                    }

                case SkillTargetType.Friendly:
                    {
                        return Units.Where(x =>
                                CurrentUnit.Unit.IsPlayerControlled == x.Unit.IsPlayerControlled && !x.Unit.IsDead)
                            .ToList();
                    }

                case SkillTargetType.Self:
                    return new[] { CurrentUnit };

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
                    unitToRestoreMana.Unit.RestoreManaPoint();
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
        }

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