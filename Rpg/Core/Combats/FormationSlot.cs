namespace Core.Combats;

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