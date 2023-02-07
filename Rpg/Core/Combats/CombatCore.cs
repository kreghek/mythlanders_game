using Core.Dices;

namespace Core.Combats;

public class CombatCore
{
    private readonly IDice _dice;
    private readonly CombatField _combatField;

    public CombatCore(IDice dice)
    {
        _dice = dice;
        _combatField = new CombatField();
    }

    public void Initialize(IReadOnlyCollection<FormationSlot> heroes, IReadOnlyCollection<FormationSlot> monsters)
    {
        InitializeCombatFieldSide(heroes, _combatField.HeroSide);
        InitializeCombatFieldSide(monsters, _combatField.MonsterSide);
    }

    private void InitializeCombatFieldSide(IReadOnlyCollection<FormationSlot> heroes, FormationSlot[,] side)
    {
        foreach (var hero in heroes)
        {
            side[hero.Index % 2, hero.Index / 2].Combatant = hero.Combatant;
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
                HeroSide[columnIndex, lineIndex] = new FormationSlot(slotIndex);
                MonsterSide[columnIndex, lineIndex] = new FormationSlot(slotIndex);
            }
        }
    }
}

public class FormationSlot
{
    public FormationSlot(int index)
    {
        Index = index;
    }

    public int Index { get; }
    public Combatant? Combatant { get; set; }
}