namespace Core.Combats;

public static class CombatFieldSideExtensions
{
    public static IReadOnlyCollection<Combatant> GetAllCombatants(this CombatFieldSide side)
    {
        return GetCombatantsIterator(side).ToArray();
    }

    public static FieldCoords GetCombatantCoords(this CombatFieldSide side, Combatant combatant)
    {
        for (var colIndex = 0; colIndex < side.ColumnCount; colIndex++)
        {
            for (var lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
            {
                var fieldCoords = new FieldCoords(colIndex, lineIndex);
                if (side[fieldCoords].Combatant == combatant)
                {
                    return fieldCoords;
                }
            }
        }

        throw new ArgumentException("Not found", nameof(combatant));
    }

    private static IEnumerable<Combatant> GetCombatantsIterator(CombatFieldSide side)
    {
        for (var colIndex = 0; colIndex < side.ColumnCount; colIndex++)
        {
            for (var lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
            {
                var slot = side[new FieldCoords(colIndex, lineIndex)].Combatant;
                if (slot is not null)
                {
                    yield return slot;
                }
            }
        }
    }
}