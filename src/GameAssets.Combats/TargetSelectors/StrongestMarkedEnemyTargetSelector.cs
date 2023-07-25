using CombatDicesTeam.Combats;

using Core.Combats.CombatantStatuses;

namespace Core.Combats.TargetSelectors;

public sealed class StrongestMarkedEnemyTargetSelector : MostEnemyStatValueTargetSelectorBase, ITargetSelector
{
    public override IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        var enemies = context.EnemySide.GetAllCombatants()
            .Where(x => x.Statuses.Any(effect => effect is MarkCombatantStatus))
            .ToArray();

        if (enemies.Any())
        {
            return new[]
            {
                enemies.OrderByDescending(x => GetStatCurrentValue(x, CombatantStatTypes.HitPoints))
                    .First()
            };
        }

        return Array.Empty<ICombatant>();
    }
}