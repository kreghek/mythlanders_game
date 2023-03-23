namespace Core.Combats.TargetSelectors;

public abstract class MostEnemyStatValueTargetSelectorBase
{
    protected static int GetStatCurrentValue(Combatant x, UnitStatType statType)
    {
        return x.Stats.Single(x => x.Type == statType).Value.Current;
    }
}