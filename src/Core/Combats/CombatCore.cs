using System.Collections.ObjectModel;

using Core.Dices;

namespace Core.Combats;

public class CombatCore
{
    private readonly IList<Combatant> _allCombatantList;

    private readonly IDice _dice;
    private readonly IList<Combatant> _roundQueue;

    public CombatCore(IDice dice)
    {
        _dice = dice;
        Field = new CombatField();

        _allCombatantList = new Collection<Combatant>();
        _roundQueue = new List<Combatant>();
    }

    /// <summary>
    /// Current active combatant.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public Combatant CurrentCombatant => _roundQueue.FirstOrDefault() ?? throw new InvalidOperationException();

    /// <summary>
    /// All combatants in the combat.
    /// </summary>
    public IReadOnlyCollection<Combatant> CurrentCombatants => _allCombatantList.ToArray();

    /// <summary>
    /// Combat field.
    /// </summary>
    public CombatField Field { get; }

    /// <summary>
    /// Check the combat is finished.
    /// </summary>
    //TODO Change to combat state (via interface) - inprogress, victory, failure, draw in current game. 
    public bool Finished
    {
        get
        {
            var aliveUnits = _allCombatantList.Where(x => !x.IsDead).ToArray();
            var playerUnits = aliveUnits.Where(x => x.IsPlayerControlled);
            var hasPlayerUnits = playerUnits.Any();

            var cpuUnits = aliveUnits.Where(x => !x.IsPlayerControlled);
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

    /// <summary>
    /// Current combat queue of turns.
    /// </summary>
    public IReadOnlyList<Combatant> RoundQueue => _roundQueue.ToArray();

    /// <summary>
    /// Complete current turn of combat.
    /// Use in the end of combat movement execution.
    /// </summary>
    public void CompleteTurn()
    {
        var context = new CombatantEffectLifetimeDispelContext(this);

        CombatantEndsTurn?.Invoke(this, new CombatantEndsTurnEventArgs(CurrentCombatant));

        CurrentCombatant.UpdateEffects(CombatantStatusUpdateType.EndCombatantTurn, context);

        if (_roundQueue.Any())
        {
            RemoveCurrentCombatantFromRoundQueue();
        }

        while (true)
        {
            if (!_roundQueue.Any())
            {
                UpdateAllCombatantEffects(CombatantStatusUpdateType.EndRound, context);

                if (Finished)
                {
                    var combatResult = CalcResult();
                    CombatFinished?.Invoke(this, new CombatFinishedEventArgs(combatResult));
                    return;
                }

                StartRound(context);

                CombatantStartsTurn?.Invoke(this, new CombatantTurnStartedEventArgs(CurrentCombatant));

                return;
            }

            if (_roundQueue.First().IsDead)
            {
                RemoveCurrentCombatantFromRoundQueue();
            }
            else
            {
                if (Finished)
                {
                    var combatResult = CalcResult();
                    CombatFinished?.Invoke(this, new CombatFinishedEventArgs(combatResult));
                    return;
                }

                break;
            }
        }

        CurrentCombatant.UpdateEffects(CombatantStatusUpdateType.StartCombatantTurn, context);

        CombatantStartsTurn?.Invoke(this, new CombatantTurnStartedEventArgs(CurrentCombatant));
    }

    /// <summary>
    /// Create combat move execution to visualize and apply effects in the right way.
    /// </summary>
    public CombatMovementExecution CreateCombatMovementExecution(CombatMovementInstance movement)
    {
        CurrentCombatant.Stats.Single(x => x.Type == CombatantStatTypes.Resolve).Value.Consume(1);

        var handSlotIndex = CurrentCombatant.DropMovementFromHand(movement);

        if (handSlotIndex is not null)
        {
            CombatantUsedMove?.Invoke(this,
                new CombatantHandChangedEventArgs(CurrentCombatant, movement, handSlotIndex.Value));
        }

        var effectContext =
            new EffectCombatContext(CurrentCombatant, Field, _dice, HandleCombatantDamagedToStat,
                HandleSwapFieldPositions, this);

        var effectImposeItems = new List<CombatEffectImposeItem>();

        foreach (var effectInstance in movement.Effects)
        {
            var effectInstanceClosure = effectInstance;

            void EffectInfluenceDelegate(Combatant materializedTarget)
            {
                effectInstanceClosure.Influence(materializedTarget, effectContext);
            }

            var effectTargets = effectInstance.Selector.GetMaterialized(CurrentCombatant, GetCurrentSelectorContext());

            if (movement.SourceMovement.Tags.HasFlag(CombatMovementTags.Attack))
            {
                foreach (var effectTarget in effectTargets)
                {
                    if (effectTarget == CurrentCombatant)
                    {
                        // Does not defence against yourself.
                        continue;
                    }

                    var targetDefenseMovement = GetAutoDefenseMovement(effectTarget);
                    var targetIsInQueue = _roundQueue.Any(x => x == effectTarget);

                    if (targetDefenseMovement is not null && targetIsInQueue)
                    {
                        foreach (var autoDefenseEffect in targetDefenseMovement.AutoDefenseEffects)
                        {
                            void AutoEffectInfluenceDelegate(Combatant materializedTarget)
                            {
                                autoDefenseEffect.Influence(materializedTarget, effectContext);
                            }

                            var autoDefenseEffectTargets =
                                effectInstance.Selector.GetMaterialized(effectTarget, GetSelectorContext(effectTarget));

                            var autoDefenseEffectImposeItem =
                                new CombatEffectImposeItem(AutoEffectInfluenceDelegate, autoDefenseEffectTargets);

                            effectImposeItems.Add(autoDefenseEffectImposeItem);
                        }

                        _roundQueue.Remove(effectTarget);
                        var autoHandSlotIndex = effectTarget.DropMovementFromHand(targetDefenseMovement);

                        if (autoHandSlotIndex is not null)
                        {
                            CombatantUsedMove?.Invoke(this,
                                new CombatantHandChangedEventArgs(effectTarget, targetDefenseMovement,
                                    autoHandSlotIndex.Value));
                        }
                    }
                }
            }

            var effectImposeItem = new CombatEffectImposeItem(EffectInfluenceDelegate, effectTargets);

            // Play auto-defence effects before an attacks.
            effectImposeItems.Add(effectImposeItem);
        }

        void CompleteSkillAction()
        {
        }

        var movementExecution = new CombatMovementExecution(CompleteSkillAction, effectImposeItems);

        return movementExecution;
    }

    public void DispelCombatantEffect(Combatant targetCombatant, ICombatantStatus combatantEffect)
    {
        targetCombatant.RemoveEffect(combatantEffect, new CombatantEffectLifetimeDispelContext(this));
        CombatantEffectHasBeenDispeled?.Invoke(this, new CombatantEffectEventArgs(targetCombatant, combatantEffect));
    }

    public int HandleCombatantDamagedToStat(Combatant combatant, ICombatantStatType statType, int damageAmount)
    {
        var (remains, wasTaken) = TakeStat(combatant, statType, damageAmount);

        if (wasTaken)
        {
            CombatantHasBeenDamaged?.Invoke(this, new CombatantDamagedEventArgs(combatant, statType, damageAmount));
        }

        if (combatant.Stats.Single(x => x.Type == CombatantStatTypes.HitPoints).Value.Current <= 0)
        {
            var shiftShape = DetectShapeShifting();
            if (shiftShape)
            {
                CombatantShiftShaped?.Invoke(this, new CombatantShiftShapedEventArgs(combatant));
            }

            combatant.SetDead();
            CombatantHasBeenDefeated?.Invoke(this, new CombatantDefeatedEventArgs(combatant));

            var targetSide = GetTargetSide(combatant, Field);
            var coords = targetSide.GetCombatantCoords(combatant);
            targetSide[coords].Combatant = null;
        }

        return remains;
    }

    public void ImposeCombatantEffect(Combatant targetCombatant, ICombatantStatus combatantEffect)
    {
        targetCombatant.AddEffect(combatantEffect, new CombatantEffectImposeContext(this),
            new CombatantEffectLifetimeImposeContext(targetCombatant, this));
        CombatantEffectHasBeenImposed?.Invoke(this, new CombatantEffectEventArgs(targetCombatant, combatantEffect));
    }

    /// <summary>
    /// Initialize combat.
    /// </summary>
    /// <param name="heroes">Combatants of hero side (player)</param>
    /// <param name="monsters">Combatants of monster side (CPU).</param>
    public void Initialize(IReadOnlyCollection<FormationSlot> heroes, IReadOnlyCollection<FormationSlot> monsters)
    {
        InitializeCombatFieldSide(heroes, Field.HeroSide);
        InitializeCombatFieldSide(monsters, Field.MonsterSide);

        foreach (var combatant in _allCombatantList)
        {
            combatant.PrepareToCombat(this);
        }

        var context = new CombatantEffectLifetimeDispelContext(this);
        StartRound(context);

        CombatantStartsTurn?.Invoke(this, new CombatantTurnStartedEventArgs(CurrentCombatant));
    }

    /// <summary>
    /// Use fo stun.
    /// </summary>
    public void Interrupt()
    {
        CombatantInterrupted?.Invoke(this, new CombatantInterruptedEventArgs(CurrentCombatant));
        CompleteTurn();
    }

    /// <summary>
    /// Maneuvering of combatant.
    /// </summary>
    /// <param name="combatStepDirection"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void UseManeuver(CombatStepDirection combatStepDirection)
    {
        var currentCoords = GetCurrentCoords();

        var targetCoords = GetCoordsByDirection(combatStepDirection, currentCoords);

        var side = GetCurrentSelectorContext().ActorSide;

        HandleSwapFieldPositions(currentCoords, side, targetCoords, side);

        CurrentCombatant.Stats.Single(x => x.Type == CombatantStatTypes.Maneuver).Value.Consume(1);
    }

    /// <summary>
    /// Used by combatants to restore Resolve stat.
    /// </summary>
    public void Wait()
    {
        RestoreStatOfAllCombatants(CombatantStatTypes.Resolve);

        CompleteTurn();
    }

    private CombatFinishResult CalcResult()
    {
        var aliveUnits = _allCombatantList.Where(x => !x.IsDead).ToArray();
        var playerUnits = aliveUnits.Where(x => x.IsPlayerControlled);
        var hasPlayerUnits = playerUnits.Any();

        var cpuUnits = aliveUnits.Where(x => !x.IsPlayerControlled);
        var hasCpuUnits = cpuUnits.Any();

        // TODO Looks like XOR
        if (hasPlayerUnits && !hasCpuUnits)
        {
            return CombatFinishResult.HeroesAreWinners;
        }

        if (!hasPlayerUnits && hasCpuUnits)
        {
            return CombatFinishResult.MonstersAreWinners;
        }

        return CombatFinishResult.Draw;
    }

    private bool DetectShapeShifting()
    {
        return false;
    }

    private void DoCombatantHasBeenAdded(CombatFieldSide targetSide, FieldCoords targetCoords, Combatant combatant)
    {
        var combatFieldInfo = new CombatFieldInfo(targetSide, targetCoords);
        var args = new CombatantHasBeenAddedEventArgs(combatant, combatFieldInfo);
        CombatantHasBeenAdded?.Invoke(this, args);
    }

    private static CombatMovementInstance? GetAutoDefenseMovement(Combatant target)
    {
        return target.Hand.FirstOrDefault(x =>
            x != null && x.SourceMovement.Tags.HasFlag(CombatMovementTags.AutoDefense));
    }

    private static FieldCoords GetCoordsByDirection(CombatStepDirection combatStepDirection, FieldCoords currentCoords)
    {
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
        return targetCoords;
    }

    private FieldCoords GetCurrentCoords()
    {
        var side = GetCurrentSelectorContext().ActorSide;

        for (var col = 0; col < side.ColumnCount; col++)
        {
            for (var lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
            {
                if (CurrentCombatant == side[new FieldCoords(col, lineIndex)].Combatant)
                {
                    return new FieldCoords(col, lineIndex);
                }
            }
        }

        throw new InvalidOperationException();
    }

    private ITargetSelectorContext GetCurrentSelectorContext()
    {
        return GetSelectorContext(CurrentCombatant);
    }

    private ITargetSelectorContext GetSelectorContext(Combatant combatant)
    {
        if (combatant.IsPlayerControlled)
        {
            return new TargetSelectorContext(Field.HeroSide, Field.MonsterSide, _dice);
        }

        return new TargetSelectorContext(Field.MonsterSide, Field.HeroSide, _dice);
    }

    private static CombatFieldSide GetTargetSide(Combatant target, CombatField field)
    {
        try
        {
            var _ = field.HeroSide.GetCombatantCoords(target);
            return field.HeroSide;
        }
        catch (ArgumentException)
        {
            var _ = field.MonsterSide.GetCombatantCoords(target);
            return field.MonsterSide;
        }
    }

    private void HandleSwapFieldPositions(FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide)
    {
        if (sourceCoords == destinationCoords && sourceFieldSide == destinationFieldSide)
        {
            return;
        }

        var sourceCombatant = sourceFieldSide[sourceCoords].Combatant;
        var targetCombatant = destinationFieldSide[destinationCoords].Combatant;

        destinationFieldSide[destinationCoords].Combatant = sourceCombatant;

        if (sourceCombatant is not null)
        {
            CombatantHasChangePosition?.Invoke(this,
                new CombatantHasChangedPositionEventArgs(sourceCombatant, destinationFieldSide, destinationCoords));
        }

        sourceFieldSide[sourceCoords].Combatant = targetCombatant;

        if (targetCombatant is not null)
        {
            CombatantHasChangePosition?.Invoke(this,
                new CombatantHasChangedPositionEventArgs(targetCombatant, sourceFieldSide, sourceCoords));
        }
    }

    private void InitializeCombatFieldSide(IReadOnlyCollection<FormationSlot> formationSlots, CombatFieldSide side)
    {
        foreach (var slot in formationSlots)
        {
            if (slot.Combatant is null)
            {
                continue;
            }

            var coords = new FieldCoords(slot.ColumnIndex, slot.LineIndex);
            side[coords].Combatant = slot.Combatant;

            _allCombatantList.Add(slot.Combatant);

            DoCombatantHasBeenAdded(targetSide: side, targetCoords: coords, slot.Combatant);
        }
    }

    private void MakeUnitRoundQueue()
    {
        _roundQueue.Clear();

        var orderedByResolve = _allCombatantList
            .Where(x => !x.IsDead)
            .OrderByDescending(x => x.Stats.Single(s => s.Type == CombatantStatTypes.Resolve).Value.Current)
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

    private void RemoveCurrentCombatantFromRoundQueue()
    {
        _roundQueue.RemoveAt(0);
    }

    private void RestoreCombatantHand(Combatant combatant)
    {
        for (var handSlotIndex = 0; handSlotIndex < combatant.Hand.Count; handSlotIndex++)
        {
            if (combatant.Hand[handSlotIndex] is null)
            {
                var nextMove = combatant.PopNextPoolMovement();
                if (nextMove is null)
                {
                    break;
                }

                combatant.AssignMoveToHand(handSlotIndex, nextMove);
                CombatantAssignedNewMove?.Invoke(this,
                    new CombatantHandChangedEventArgs(combatant, nextMove, handSlotIndex));
            }
        }
    }

    private void RestoreHandsOfAllCombatants()
    {
        foreach (var combatant in _allCombatantList)
        {
            RestoreCombatantHand(combatant);
        }
    }

    private void RestoreManeuversOfAllCombatants()
    {
        RestoreStatOfAllCombatants(CombatantStatTypes.Maneuver);
    }

    private void RestoreShieldsOfAllCombatants()
    {
        RestoreStatOfAllCombatants(CombatantStatTypes.ShieldPoints);
    }

    private void RestoreStatOfAllCombatants(ICombatantStatType statType)
    {
        var combatants = _allCombatantList.Where(x => !x.IsDead);
        foreach (var combatant in combatants)
        {
            var stat = combatant.Stats.Single(x => x.Type == statType);
            var valueToRestore = stat.Value.ActualMax - stat.Value.Current;
            stat.Value.Restore(valueToRestore);
        }
    }

    private void StartRound(ICombatantStatusLifetimeDispelContext combatantEffectLifetimeDispelContext)
    {
        MakeUnitRoundQueue();
        RestoreHandsOfAllCombatants();
        RestoreShieldsOfAllCombatants();
        RestoreManeuversOfAllCombatants();

        UpdateAllCombatantEffects(CombatantStatusUpdateType.StartRound, combatantEffectLifetimeDispelContext);
        CurrentCombatant.UpdateEffects(CombatantStatusUpdateType.StartCombatantTurn,
            combatantEffectLifetimeDispelContext);
    }

    private static (int result, bool isTaken) TakeStat(Combatant combatant, ICombatantStatType statType, int value)
    {
        var stat = combatant.Stats.SingleOrDefault(x => x.Type == statType);

        if (stat is null)
        {
            return (value, false);
        }

        var d = Math.Min(value, stat.Value.Current);
        stat.Value.Consume(d);

        var remains = value - d;

        return (remains, d > 0);
    }

    private void UpdateAllCombatantEffects(CombatantStatusUpdateType updateType,
        ICombatantStatusLifetimeDispelContext context)
    {
        foreach (var combatant in _allCombatantList)
        {
            if (!combatant.IsDead)
            {
                combatant.UpdateEffects(updateType, context);
            }
        }
    }

    public event EventHandler<CombatantHasBeenAddedEventArgs>? CombatantHasBeenAdded;
    public event EventHandler<CombatantTurnStartedEventArgs>? CombatantStartsTurn;
    public event EventHandler<CombatantEndsTurnEventArgs>? CombatantEndsTurn;
    public event EventHandler<CombatantDamagedEventArgs>? CombatantHasBeenDamaged;
    public event EventHandler<CombatantDefeatedEventArgs>? CombatantHasBeenDefeated;
    public event EventHandler<CombatantShiftShapedEventArgs>? CombatantShiftShaped;
    public event EventHandler<CombatantHasChangedPositionEventArgs>? CombatantHasChangePosition;
    public event EventHandler<CombatFinishedEventArgs>? CombatFinished;
    public event EventHandler<CombatantInterruptedEventArgs>? CombatantInterrupted;
    public event EventHandler<CombatantHandChangedEventArgs>? CombatantAssignedNewMove;
    public event EventHandler<CombatantHandChangedEventArgs>? CombatantUsedMove;
    public event EventHandler<CombatantEffectEventArgs>? CombatantEffectHasBeenImposed;
    public event EventHandler<CombatantEffectEventArgs>? CombatantEffectHasBeenDispeled;
}