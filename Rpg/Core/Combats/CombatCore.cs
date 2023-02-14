using System.Collections.ObjectModel;

using Core.Dices;

namespace Core.Combats;

public class CombatCore
{
    private readonly IList<Combatant> _allUnitList;

    private readonly IDice _dice;
    private readonly IList<Combatant> _roundQueue;

    public CombatCore(IDice dice)
    {
        _dice = dice;
        Field = new CombatField();

        _allUnitList = new Collection<Combatant>();
        _roundQueue = new List<Combatant>();
    }

    public Combatant CurrentCombatant => _roundQueue.FirstOrDefault() ?? throw new InvalidOperationException();

    public CombatField Field { get; }

    public IReadOnlyList<Combatant> RoundQueue => _roundQueue.ToArray();

    public void CompleteTurn()
    {
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
    }

    public ITargetSelectorContext GetCurrentSelectorContext()
    {
        return GetSelectorContext(CurrentCombatant);
    }

    public void Initialize(IReadOnlyCollection<FormationSlot> heroes, IReadOnlyCollection<FormationSlot> monsters)
    {
        InitializeCombatFieldSide(heroes, Field.HeroSide);
        InitializeCombatFieldSide(monsters, Field.MonsterSide);

        foreach (var combatant in _allUnitList) combatant.StartCombat();

        StartRound();
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
            void EffectInfluenceDelegate(Combatant materializedTarget)
            {
                effect.Influence(materializedTarget, effectContext);
            }

            var effectTargets = effect.Selector.Get(CurrentCombatant, GetCurrentSelectorContext());

            if (movement.Tags.HasFlag(CombatMovementTags.Attack))
                foreach (var effectTarget in effectTargets)
                {
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
                                effect.Selector.Get(effectTarget, GetSelectorContext(effectTarget));

                            var autoDefenseEffectImposeItem =
                                new CombatEffectImposeItem(AutoEffectInfluenceDelegate, autoDefenseEffectTargets);

                            effectImposeItems.Add(autoDefenseEffectImposeItem);
                        }

                        _roundQueue.Remove(effectTarget);
                        effectTarget.DropMovement(targetDefenseMovement);
                    }
                }

            var effectImposeItem = new CombatEffectImposeItem(EffectInfluenceDelegate, effectTargets);

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

    private static CombatMovement? GetAutoDefenseMovement(Combatant target)
    {
        return target.Hand.FirstOrDefault(x => x != null && x.Tags.HasFlag(CombatMovementTags.AutoDefense));
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

                _allUnitList.Add(slot.Combatant);
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
            if (!unit.IsDead)
                _roundQueue.Add(unit);
    }

    private void RemoveCurrentCombatantFromRoundQueue()
    {
        _roundQueue.RemoveAt(0);
    }

    private void StartRound()
    {
        MakeUnitRoundQueue();

        UpdateAllCombatantEffects(CombatantEffectUpdateType.StartRound);
        CurrentCombatant.UpdateEffects(CombatantEffectUpdateType.StartCombatantTurn);
    }

    private void UpdateAllCombatantEffects(CombatantEffectUpdateType updateType)
    {
        foreach (var combatant in _allUnitList)
            if (!combatant.IsDead)
                combatant.UpdateEffects(updateType);
    }

    public event EventHandler<CombatantDamagedEventArgs>? CombatantHasBeenDamaged;
}

public enum CombatStepDirection
{
    Up,
    Down,
    Forward,
    Backward
}