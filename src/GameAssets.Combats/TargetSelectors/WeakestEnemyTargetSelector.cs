namespace Core.Combats.TargetSelectors;

public sealed class WeakestEnemyTargetSelector : MostEnemyStatValueTargetSelectorBase, ITargetSelector
{
    public override IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        var enemies = context.EnemySide.GetAllCombatants().ToArray();

        if (enemies.Any())
        {
            return new[]
            {
                enemies.OrderBy(x => GetStatCurrentValue(x, CombatantStatTypes.HitPoints))
                    .First()
            };
        }

        return Array.Empty<ICombatant>();
    }
}