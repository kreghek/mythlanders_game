namespace Core.Combats.TargetSelectors;

public sealed class AllVanguardEnemiesTargetSelector : ITargetSelector
{
    private static IEnumerable<Combatant> GetIterator(ITargetSelectorContext context)
    {
        for (var lineIndex = 0; lineIndex < context.EnemySide.LineCount; lineIndex++)
        {
            var slot = context.EnemySide[new FieldCoords(0, lineIndex)];
            if (slot.Combatant is not null) yield return slot.Combatant;
        }
    }

    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        return GetIterator(context).ToArray();
    }
}