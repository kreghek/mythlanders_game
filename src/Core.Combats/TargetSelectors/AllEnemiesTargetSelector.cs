namespace Core.Combats.TargetSelectors;

public sealed class AllEnemiesTargetSelector : ITargetSelector
{
    private static IEnumerable<Combatant> GetIterator(ITargetSelectorContext context)
    {
        return context.EnemySide.GetAllCombatants();
    }

    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        return GetIterator(context).ToArray();
    }
}