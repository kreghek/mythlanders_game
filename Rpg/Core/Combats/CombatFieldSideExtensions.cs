namespace Core.Combats;

public static class CombatFieldSideExtensions
{
    public static IReadOnlyCollection<Combatant> GetAllCombatants(this CombatFieldSide side)
    {
        return GetCombatantsIterator(side).ToArray();
    }

    private static IEnumerable<Combatant> GetCombatantsIterator(CombatFieldSide side)
    {
        for (int colIndex = 0; colIndex < side.ColumnCount; colIndex++)
        {
            for (int lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
            {
                var combatant = side[new FieldCoords(colIndex, lineIndex)].Combatant;
                if (combatant is not null)
                {
                    yield return combatant;
                }
            }
        }
    }
}