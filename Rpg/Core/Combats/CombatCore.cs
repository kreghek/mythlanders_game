using System.Collections.ObjectModel;

using Core.Dices;

namespace Core.Combats;

public class CombatCore
{
    private readonly IList<Combatant> _allUnitList;
    private readonly IList<Combatant> _roundQueue;

    private readonly IDice _dice;
    private readonly CombatField _combatField;

    private int _roundNumber;

    public event EventHandler<CombatantDamagedEventArgs>? CombatantHasBeenDamaged;

    public IReadOnlyList<Combatant> RoundQueue => _roundQueue.ToArray();

    public CombatCore(IDice dice)
    {
        _dice = dice;
        _combatField = new CombatField();

        _allUnitList = new Collection<Combatant>();
        _roundQueue = new List<Combatant>();
    }

    public void Initialize(IReadOnlyCollection<FormationSlot> heroes, IReadOnlyCollection<FormationSlot> monsters)
    {
        InitializeCombatFieldSide(heroes, _combatField.HeroSide);
        InitializeCombatFieldSide(monsters, _combatField.MonsterSide);

        foreach (var combatant in _allUnitList)
        {
            combatant.StartCombat();
        }

        StartRound();
    }

    public Combatant CurrentCombatant => _roundQueue.FirstOrDefault() ?? throw new InvalidOperationException();

    private void StartRound()
    {
        MakeUnitRoundQueue();

        _roundNumber++;

        UpdateAllCombatantEffects(CombatantEffectUpdateType.StartRound);
        CurrentCombatant.UpdateEffects(CombatantEffectUpdateType.StartCombatantTurn);
    }

    private void UpdateAllCombatantEffects(CombatantEffectUpdateType updateType)
    {
        foreach (var combatant in _allUnitList)
        {
            if (!combatant.IsDead)
            {
                combatant.UpdateEffects(updateType);
            }
        }
    }

    private void MakeUnitRoundQueue()
    {
        _roundQueue.Clear();

        var orderedByResolve = _allUnitList
            .OrderByDescending(x => x.Stats.Single(s => s.Type == UnitStatType.Resolve).Value.ActualMax)
            .ThenByDescending(x => x.IsPlayerControlled)
            .ToArray();

        foreach (var unit in orderedByResolve)
        {
            if (!unit.IsDead)
            {
                _roundQueue.Add(unit);
            }
        }
    }

    private void InitializeCombatFieldSide(IReadOnlyCollection<FormationSlot> formationSlots, CombatFieldSide side)
    {
        foreach (var slot in formationSlots)
        {
            if (slot.Combatant is not null)
            {
                var coords = new FieldCoords(slot.ColumnIndex, slot.LineIndex);
                side[coords].Combatant = slot.Combatant;

                _allUnitList.Add(slot.Combatant);
            }
        }
    }

    public CombatMovementExecution UseCombatMovement(CombatMovement movement)
    {
        var effectContext = new EffectCombatContext(Field, _dice,
            (combatant, statType, value) =>
            {
                CombatantHasBeenDamaged?.Invoke(this, new CombatantDamagedEventArgs(combatant, statType, value));
            }
        );

        var effectImposeItems = new List<CombatEffectImposeItem>();

        foreach (var effect in movement.Effects)
        {
            void EffectImposeDelegate(Combatant materializedTarget)
            {
                effect.Imposer.Impose(effect, materializedTarget, effectContext);
            }

            var effectTargets = effect.Selector.Get(CurrentCombatant, GetCurrentSelectorContext());

            if (movement.Tags.HasFlag(CombatMovementTags.Attack))
            {
                foreach (var effectTarget in effectTargets)
                {
                    var targetDefenseMovement = GetAutoDefenseMovement(effectTarget);
                    var targetIsInQueue = _roundQueue.Any(x => x == effectTarget);

                    if (targetDefenseMovement is not null && targetIsInQueue)
                    {
                        foreach (var autoDefenseEffect in targetDefenseMovement.AutoDefenseEffects)
                        {
                            void AutoEffectImposeDelegate(Combatant materializedTarget)
                            {
                                autoDefenseEffect.Imposer.Impose(autoDefenseEffect, materializedTarget, effectContext);
                            }

                            var autoDefenseEffectTargets = effect.Selector.Get(effectTarget, GetSelectorContext(effectTarget));

                            var autoDefenseEffectImposeItem = new CombatEffectImposeItem(AutoEffectImposeDelegate, autoDefenseEffectTargets);

                            effectImposeItems.Add(autoDefenseEffectImposeItem);
                        }

                        _roundQueue.Remove(effectTarget);
                        effectTarget.DropMovement(targetDefenseMovement);
                    }
                }
            }

            var effectImposeItem = new CombatEffectImposeItem(EffectImposeDelegate, effectTargets);

            effectImposeItems.Add(effectImposeItem);
        }

        void CompleteSkillAction()
        {
            CurrentCombatant.Stats.Single(x => x.Type == UnitStatType.Resolve).Value.Consume(1);

            CurrentCombatant.DropMovement(movement);
        }

        var movementExecution = new CombatMovementExecution(CompleteSkillAction, effectImposeItems);

        return movementExecution;
    }

    private static CombatMovement? GetAutoDefenseMovement(Combatant target)
    {
        return target.Hand.FirstOrDefault(x => x != null && x.Tags.HasFlag(CombatMovementTags.AutoDefense));
    }

    public ITargetSelectorContext GetCurrentSelectorContext()
    {
        return GetSelectorContext(CurrentCombatant);
    }

    private ITargetSelectorContext GetSelectorContext(Combatant combatant)
    {
        if (combatant.IsPlayerControlled)
        {
            return new TargetSelectorContext(Field.HeroSide, Field.MonsterSide);
        }
        else
        {
            return new TargetSelectorContext(Field.MonsterSide, Field.HeroSide);
        }
    }

    public void CompleteTurn()
    {
        CurrentCombatant.UpdateEffects(CombatantEffectUpdateType.EndCombatantTurn);

        if (_roundQueue.Any())
        {
            RemoveCurrentCombatantFromRoundQueue();
        }

        while (true)
        {
            if (!_roundQueue.Any())
            {
                UpdateAllCombatantEffects(CombatantEffectUpdateType.EndRound);
                StartRound();
                return;
            }

            if (_roundQueue.First().IsDead)
            {
                RemoveCurrentCombatantFromRoundQueue();
            }
            else
            {
                break;
            }
        }

        CurrentCombatant.UpdateEffects(CombatantEffectUpdateType.StartCombatantTurn);
    }

    private void RemoveCurrentCombatantFromRoundQueue()
    {
        _roundQueue.RemoveAt(0);
    }

    public CombatField Field => _combatField;

    public void UseManeuver(CombatStepDirection combatStepDirection)
    {
        var currentCoords = GetCurrentCoords();

        var targetCoords = combatStepDirection switch
        {
            CombatStepDirection.Up => currentCoords with
            {
                LineIndex = currentCoords.LineIndex - 1
            },
            CombatStepDirection.Down => currentCoords with
            {
                LineIndex = currentCoords.LineIndex + 1
            },
            CombatStepDirection.Forward => currentCoords with
            {
                ColumentIndex = currentCoords.ColumentIndex - 1
            },
            CombatStepDirection.Backward => currentCoords with
            {
                ColumentIndex = currentCoords.ColumentIndex + 1
            },
            _ => throw new ArgumentOutOfRangeException(nameof(combatStepDirection), combatStepDirection, null)
        };

        var side = GetCurrentSelectorContext().ActorSide;

        var swapCombatant = side[targetCoords].Combatant;

        side[targetCoords].Combatant = CurrentCombatant;
        side[currentCoords].Combatant = swapCombatant;

        CurrentCombatant.Stats.Single(x => x.Type == UnitStatType.Maneuver).Value.Consume(1);
    }

    private FieldCoords GetCurrentCoords()
    {
        var side = GetCurrentSelectorContext().ActorSide;

        for (int col = 0; col < side.ColumnCount; col++)
        {
            for (int lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
            {
                if (CurrentCombatant == side[new FieldCoords(col, lineIndex)].Combatant)
                {
                    return new FieldCoords(col, lineIndex);
                }
            }
        }

        throw new InvalidOperationException();
    }
}

public enum CombatStepDirection
{
    Up,
    Down,
    Forward,
    Backward
}