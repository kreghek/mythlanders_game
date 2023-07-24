namespace Core.Combats.TargetSelectors;

public sealed class SelfTargetSelector : ITargetSelector
{
    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        return new[]
        {
            actor
        };
    }
}