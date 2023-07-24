namespace Core.Combats.TargetSelectors;

public sealed class NullTargetSelector : ITargetSelector
{
    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        throw new NotImplementedException();
    }
}