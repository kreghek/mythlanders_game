namespace Core.Combats.TargetSelectors;

public sealed class WeakestEnemyTargetSelector : MostEnemyStatValueTargetSelectorBase, ITargetSelector
{
    public override IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        var enemies = context.EnemySide.GetAllCombatants().ToArray();

        if (enemies.Any())
            return new[]
            {
                enemies.OrderBy(x => GetStatCurrentValue(x, UnitStatType.HitPoints))
                    .First()
            };
        return Array.Empty<Combatant>();
    }
}