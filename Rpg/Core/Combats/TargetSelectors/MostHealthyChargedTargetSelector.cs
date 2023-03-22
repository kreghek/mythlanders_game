namespace Core.Combats.TargetSelectors;

public sealed class MostHealthyEnemyChargedTargetSelector : ITargetSelector
{
    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        var enemies = context.EnemySide.GetAllCombatants();

        return new[]
        {
            enemies.OrderByDescending(x => x.Stats.Single(x => x.Type == UnitStatType.HitPoints).Value.Current)
                .First()
        };
    }
}