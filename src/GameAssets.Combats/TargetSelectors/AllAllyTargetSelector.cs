namespace Core.Combats.TargetSelectors;

public sealed class AllAllyTargetSelector : ITargetSelector
{
    public IReadOnlyList<Combats.ICombatant> GetMaterialized(Combats.ICombatant actor, ITargetSelectorContext context)
    {
        return context.ActorSide.GetAllCombatants().ToArray();
    }
}