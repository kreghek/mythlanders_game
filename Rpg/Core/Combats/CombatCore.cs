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

    private void InitializeCombatFieldSide(IReadOnlyCollection<FormationSlot> formationSlots, FormationSlot[,] side)
    {
        foreach (var slot in formationSlots)
        {
            if (slot.Combatant is not null)
            {
                side[slot.ColumnIndex, slot.LineIndex].Combatant = slot.Combatant;

                _allUnitList.Add(slot.Combatant);
            }
        }
    }

    public CombatField Field => _combatField;
}