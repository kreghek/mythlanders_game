using Core.Combats.CombatantStatuses;

namespace Core.Combats.TargetSelectors;

public sealed class WeakestMarkedEnemyTargetSelector : MostEnemyStatValueTargetSelectorBase, ITargetSelector
{
    public override IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        var enemies = context.EnemySide.GetAllCombatants()
            .Where(x => x.Statuses.Any(effect => effect is MarkCombatantStatus))
            .ToArray();

        if (enemies.Any())
        {
            return new[]
            {
                enemies.OrderBy(x => GetStatCurrentValue(x, CombatantStatTypes.HitPoints))
                    .First()
            };
        }

        return Array.Empty<Combatant>();
    }
}