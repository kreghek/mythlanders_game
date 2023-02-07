using System.Collections.ObjectModel;

using Core.Dices;

namespace Core.Combats;

public class CombatCore
{
    private readonly IList<Combatant> _allUnitList;
    private readonly IDice _dice;
    private readonly CombatField _combatField;

    public CombatCore(IDice dice)
    {
        _dice = dice;
        _combatField = new CombatField();

        _allUnitList = new Collection<Combatant>();
    }

    public void Initialize(IReadOnlyCollection<FormationSlot> heroes, IReadOnlyCollection<FormationSlot> monsters)
    {
        InitializeCombatFieldSide(heroes, _combatField.HeroSide);
        InitializeCombatFieldSide(monsters, _combatField.MonsterSide);

        foreach (var combatant in _allUnitList)
        {
            combatant.StartCombat();
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

public class CombatField
{
    public FormationSlot[,] HeroSide { get; }
    public FormationSlot[,] MonsterSide { get; }

    public CombatField()
    {
        HeroSide = new FormationSlot[2, 3];
        MonsterSide = new FormationSlot[2, 3];

        for (var columnIndex = 0; columnIndex < 2; columnIndex++)
        {
            for (var lineIndex = 0; lineIndex < 3; lineIndex++)
            {
                var slotIndex = lineIndex + columnIndex;
                HeroSide[columnIndex, lineIndex] = new FormationSlot(columnIndex, lineIndex);
                MonsterSide[columnIndex, lineIndex] = new FormationSlot(columnIndex, lineIndex);
            }
        }
    }
}

public class FormationSlot
{
    public int ColumnIndex { get; }
    public int LineIndex { get; }

    public FormationSlot(int columnIndex, int lineIndex)
    {
        ColumnIndex = columnIndex;
        LineIndex = lineIndex;
    }

    public Combatant? Combatant { get; set; }
}