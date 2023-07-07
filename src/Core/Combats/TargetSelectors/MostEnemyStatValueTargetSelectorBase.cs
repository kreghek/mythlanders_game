namespace Core.Combats.TargetSelectors;

public abstract class MostEnemyStatValueTargetSelectorBase : ITargetSelector
{
    protected static int GetStatCurrentValue(Combatant combatant, ICombatantStatType statType)
    {
        return combatant.Stats.Single(x => x.Type == statType).Value.Current;
    }

    public abstract IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context);
}