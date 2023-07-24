namespace Core.Combats.TargetSelectors;

public sealed class AllEnemiesTargetSelector : ITargetSelector
{
    private static IEnumerable<Combats.ICombatant> GetIterator(ITargetSelectorContext context)
    {
        return context.EnemySide.GetAllCombatants();
    }

    public IReadOnlyList<Combats.ICombatant> GetMaterialized(Combats.ICombatant actor, ITargetSelectorContext context)
    {
        return GetIterator(context).ToArray();
    }
}