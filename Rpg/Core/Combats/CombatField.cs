namespace Core.Combats;

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
                HeroSide[columnIndex, lineIndex] = new FormationSlot(columnIndex, lineIndex);
                MonsterSide[columnIndex, lineIndex] = new FormationSlot(columnIndex, lineIndex);
            }
        }
    }
}