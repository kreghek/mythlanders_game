namespace Core.Combats.TargetSelectors;

public abstract class MostEnemyStatValueTargetSelectorBase : ITargetSelector
{
    protected static int GetStatCurrentValue(ICombatant combatant, ICombatantStatType statType)
    {
        return combatant.Stats.Single(x => x.Type == statType).Value.Current;
    }

    public abstract IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context);
}