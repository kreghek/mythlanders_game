using Core.Combats.CombatantEffects;

namespace Core.Combats.TargetSelectors;

public sealed class WeakestMarkedEnemyTargetSelector : MostEnemyStatValueTargetSelectorBase, ITargetSelector
{
    public override IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        var enemies = context.EnemySide.GetAllCombatants()
            .Where(x => x.Effects.Any(effect => effect is MarkCombatantEffect))
            .ToArray();

        if (enemies.Any())
        {
            return new[]
            {
                enemies.OrderBy(x => GetStatCurrentValue(x, UnitStatType.HitPoints))
                    .First()
            };
        }

        return Array.Empty<Combatant>();
    }
}