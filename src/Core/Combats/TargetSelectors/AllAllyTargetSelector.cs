namespace Core.Combats.TargetSelectors;

public sealed class AllAllyTargetSelector : ITargetSelector
{
    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        return context.ActorSide.GetAllCombatants().ToArray();
    }
}