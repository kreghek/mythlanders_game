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

    public Combatant CurrentCombatant => _roundQueue.FirstOrDefault() ?? throw new InvalidOperationException();

    public CombatField Field { get; }

    public IReadOnlyList<Combatant> RoundQueue => _roundQueue.ToArray();

    public void CompleteTurn()
    {
        CombatantEndsTurn?.Invoke(this, new CombatantEndsTurnEventArgs(CurrentCombatant));

        CurrentCombatant.UpdateEffects(CombatantEffectUpdateType.EndCombatantTurn);

        if (_roundQueue.Any()) RemoveCurrentCombatantFromRoundQueue();

        while (true)
        {
            if (!_roundQueue.Any())
            {
                UpdateAllCombatantEffects(CombatantEffectUpdateType.EndRound);
                StartRound();
                return;
            }

            if (_roundQueue.First().IsDead)
                RemoveCurrentCombatantFromRoundQueue();
            else
                break;
        }

        CurrentCombatant.UpdateEffects(CombatantEffectUpdateType.StartCombatantTurn);

        CombatantStartsTurn?.Invoke(this, new CombatantTurnStartedEventArgs(CurrentCombatant));
    }

    public event EventHandler<CombatantTurnStartedEventArgs>? CombatantStartsTurn;
    public event EventHandler<CombatantEndsTurnEventArgs>? CombatantEndsTurn;

    public ITargetSelectorContext GetCurrentSelectorContext()
    {
        return GetSelectorContext(CurrentCombatant);
    }

    public void Initialize(IReadOnlyCollection<FormationSlot> heroes, IReadOnlyCollection<FormationSlot> monsters)
    {
        InitializeCombatFieldSide(heroes, Field.HeroSide);
        InitializeCombatFieldSide(monsters, Field.MonsterSide);

        foreach (var combatant in _allCombatantList) combatant.StartCombat();

        StartRound();
    }

    public CombatMovementExecution UseCombatMovement(CombatMovementInstance movement)
    {
        var effectContext = new EffectCombatContext(Field, _dice, HandleCombatantDamaged);

        var effectImposeItems = new List<CombatEffectImposeItem>();

        foreach (var effectInstance in movement.Effects)
        {
            void EffectInfluenceDelegate(Combatant materializedTarget)
            {
                effectInstance.Influence(materializedTarget, effectContext);
            }

            var effectTargets = effectInstance.Selector.Get(CurrentCombatant, GetCurrentSelectorContext());

            if (movement.SourceMovement.Tags.HasFlag(CombatMovementTags.Attack))
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
                                effectInstance.Selector.Get(effectTarget, GetSelectorContext(effectTarget));

                            var autoDefenseEffectImposeItem =
                                new CombatEffectImposeItem(AutoEffectInfluenceDelegate, autoDefenseEffectTargets);

                            effectImposeItems.Add(autoDefenseEffectImposeItem);
                        }

                        _roundQueue.Remove(effectTarget);
                        effectTarget.DropMovementFromHand(targetDefenseMovement);
                    }
                }

            var effectImposeItem = new CombatEffectImposeItem(EffectInfluenceDelegate, effectTargets);

            // Play auto-defence effects before an attacks.
            effectImposeItems.Add(effectImposeItem);
        }

        void CompleteSkillAction()
        {
            CurrentCombatant.Stats.Single(x => x.Type == UnitStatType.Resolve).Value.Consume(1);

            CurrentCombatant.DropMovementFromHand(movement);
        }

        var movementExecution = new CombatMovementExecution(CompleteSkillAction, effectImposeItems);

        return movementExecution;
    }

    private void HandleCombatantDamaged(Combatant combatant, UnitStatType statType, int value)
    {
        CombatantHasBeenDamaged?.Invoke(this, new CombatantDamagedEventArgs(combatant, statType, value));

        if (combatant.Stats.Single(x => x.Type == UnitStatType.HitPoints).Value.Current <= 0)
        {
            combatant.SetDead();
            CombatantHasBeenDefeated?.Invoke(this, new CombatantDefeatedEventArgs(combatant));

            var targetSide = GetTargetSide(combatant, Field);
            var coords = targetSide.GetCombatantCoords(combatant);
            targetSide[coords].Combatant = null;
        }
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

    private static CombatMovementInstance? GetAutoDefenseMovement(Combatant target)
    {
        return target.Hand.FirstOrDefault(x => x != null && x.SourceMovement.Tags.HasFlag(CombatMovementTags.AutoDefense));
    }

    private FieldCoords GetCurrentCoords()
    {
        var side = GetCurrentSelectorContext().ActorSide;

        for (var col = 0; col < side.ColumnCount; col++)
            for (var lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
                if (CurrentCombatant == side[new FieldCoords(col, lineIndex)].Combatant)
                    return new FieldCoords(col, lineIndex);

        throw new InvalidOperationException();
    }

    private ITargetSelectorContext GetSelectorContext(Combatant combatant)
    {
        if (combatant.IsPlayerControlled)
            return new TargetSelectorContext(Field.HeroSide, Field.MonsterSide, _dice);
        return new TargetSelectorContext(Field.MonsterSide, Field.HeroSide, _dice);
    }

    private void InitializeCombatFieldSide(IReadOnlyCollection<FormationSlot> formationSlots, CombatFieldSide side)
    {
        foreach (var slot in formationSlots)
            if (slot.Combatant is not null)
            {
                var coords = new FieldCoords(slot.ColumnIndex, slot.LineIndex);
                side[coords].Combatant = slot.Combatant;

                _allCombatantList.Add(slot.Combatant);
            }
    }

    private void MakeUnitRoundQueue()
    {
        _roundQueue.Clear();

        var orderedByResolve = _allCombatantList
            .Where(x => !x.IsDead)
            .OrderByDescending(x => x.Stats.Single(s => s.Type == UnitStatType.Resolve).Value.Current)
            .ThenByDescending(x => x.IsPlayerControlled)
            .ToArray();

        foreach (var unit in orderedByResolve)
            if (!unit.IsDead)
                _roundQueue.Add(unit);
    }

    public void Wait()
    {
        RestoreStatOfAllCombatants(UnitStatType.Resolve);

        CompleteTurn();
    }

    private void RemoveCurrentCombatantFromRoundQueue()
    {
        _roundQueue.RemoveAt(0);
    }

    private void StartRound()
    {
        MakeUnitRoundQueue();
        RestoreShieldsOfAllCombatants();
        RestoreManeuversOfAllCombatants();
        RestoreHands();

        UpdateAllCombatantEffects(CombatantEffectUpdateType.StartRound);
        CurrentCombatant.UpdateEffects(CombatantEffectUpdateType.StartCombatantTurn);
    }

    private void RestoreManeuversOfAllCombatants()
    {
        RestoreStatOfAllCombatants(UnitStatType.Maneuver);
    }

    private void RestoreHands()
    {
        foreach (var combatant in _allCombatantList)
        {
            for (var handSlotIndex = 0; handSlotIndex < combatant.Hand.Count; handSlotIndex++)
            {
                if (combatant.Hand[handSlotIndex] is null)
                {
                    var nextMove = combatant.PopNextPoolMovement();
                    if (nextMove is not null)
                    {
                        combatant.AssignMoveToHand(handSlotIndex, nextMove);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    private void RestoreShieldsOfAllCombatants()
    {
        RestoreStatOfAllCombatants(UnitStatType.ShieldPoints);
    }

    private void RestoreStatOfAllCombatants(UnitStatType statType)
    {
        var combatants = _allCombatantList.Where(x => !x.IsDead);
        foreach (var combatant in combatants)
        {
            var stat = combatant.Stats.Single(x => x.Type == statType);
            var valueToRestore = stat.Value.ActualMax - stat.Value.Current;
            stat.Value.Restore(valueToRestore);
        }
    }

    private void UpdateAllCombatantEffects(CombatantEffectUpdateType updateType)
    {
        foreach (var combatant in _allCombatantList)
            if (!combatant.IsDead)
                combatant.UpdateEffects(updateType);
    }

    public event EventHandler<CombatantDamagedEventArgs>? CombatantHasBeenDamaged;
    public event EventHandler<CombatantDefeatedEventArgs>? CombatantHasBeenDefeated;
}