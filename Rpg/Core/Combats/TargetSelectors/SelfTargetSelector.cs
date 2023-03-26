namespace Core.Combats.TargetSelectors;

public sealed class SelfTargetSelector : ITargetSelector
{
    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        return new[]
        {
            actor
        };
    }
}