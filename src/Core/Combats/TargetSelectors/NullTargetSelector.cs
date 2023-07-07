namespace Core.Combats.TargetSelectors;

public sealed class NullTargetSelector : ITargetSelector
{
    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        throw new NotImplementedException();
    }
}
