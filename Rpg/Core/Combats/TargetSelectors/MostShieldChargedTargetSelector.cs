namespace Core.Combats.TargetSelectors;

public sealed class MostShieldChargedTargetSelector : ITargetSelector
{
    public IReadOnlyList<Combatant> Get(Combatant actor, ITargetSelectorContext context)
    {
        var enemies = context.EnemySide.GetAllCombatants();

        return new[] { enemies.OrderByDescending(x => x.Stats.Single(x => x.Type == UnitStatType.ShieldPoints).Value.Current).First() };
    }
}