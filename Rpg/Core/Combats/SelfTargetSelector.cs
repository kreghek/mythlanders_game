namespace Core.Combats;

public sealed class SelfTargetSelector : ITargetSelector
{
    public IReadOnlyList<Combatant> Get(Combatant actor, ITargetSelectorContext context)
    {
        return new[] { actor };
    }
}
