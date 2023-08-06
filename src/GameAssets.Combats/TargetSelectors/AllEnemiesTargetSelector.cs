using CombatDicesTeam.Combats;

namespace Core.Combats.TargetSelectors;

public sealed class AllEnemiesTargetSelector : ITargetSelector
{
    private static IEnumerable<ICombatant> GetIterator(ITargetSelectorContext context)
    {
        return context.EnemySide.GetAllCombatants();
    }

    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        return GetIterator(context).ToArray();
    }
}