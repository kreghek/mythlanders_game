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
        var effectContext = new EffectCombatContext(Field, _dice);

        var effectImposeItems = new List<CombatEffectImposeItem>();

        foreach (var effect in movement.Effects)
        {
            void effectImposeDelegate(Combatant materializedTarget)
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
                            void autoEffectImposeDelegate(Combatant materializedTarget)
                            {
                                autoDefenseEffect.Imposer.Impose(autoDefenseEffect, materializedTarget, effectContext);
                            }

                            var autoDefenseEffectTargets = effect.Selector.Get(effectTarget, GetSelectorContext(effectTarget));

                            var autoDefenseEffectImposeItem = new CombatEffectImposeItem(autoEffectImposeDelegate, effectTargets);

                            effectImposeItems.Add(autoDefenseEffectImposeItem);
                        }

                        _roundQueue.Remove(effectTarget);
                        effectTarget.DropMovement(targetDefenseMovement);
                    }
                }
            }

            var effectImposeItem = new CombatEffectImposeItem(effectImposeDelegate, effectTargets);

            effectImposeItems.Add(effectImposeItem);
        }

        void completeSkillAction()
        {
            CurrentCombatant.Stats.Single(x=>x.Type == UnitStatType.Resolve).Value.Consume(1);

            CurrentCombatant.DropMovement(movement);
        }

        var movementExecution = new CombatMovementExecution(completeSkillAction, effectImposeItems);

        return movementExecution;
    }

    private static CombatMovement? GetAutoDefenseMovement(Combatant target)
    {
        return target.Hand.First(x => x.Tags.HasFlag(CombatMovementTags.AutoDefense));
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
        if (_roundQueue.Any())
        {
            RemoveCurrentCombatantFromRoundQueue();
        }

        while (true)
        {
            if (!_roundQueue.Any())
            {
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
    }

    private void RemoveCurrentCombatantFromRoundQueue()
    {
        _roundQueue.RemoveAt(0);
    }

    public CombatField Field => _combatField;

    public void UseCombatStep(CombatStepDirection combatStepDirection)
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

        side[targetCoords].Combatant = CurrentCombatant;
        side[currentCoords].Combatant = null;
        
        CurrentCombatant.Stats.Single(x=>x.Type == UnitStatType.Maneuver).Value.Consume(1);
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