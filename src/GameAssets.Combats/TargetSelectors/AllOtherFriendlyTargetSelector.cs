namespace Core.Combats.TargetSelectors;

public sealed class AllOtherFriendlyTargetSelector : ITargetSelector
{
    private IEnumerable<Combats.ICombatant> GetIterator(ITargetSelectorContext context)
    {
        for (var lineIndex = 0; lineIndex < context.EnemySide.LineCount; lineIndex++)
        {
            var slot = context.ActorSide[new FieldCoords(0, lineIndex)];
            if (slot.Combatant is not null)
            {
                yield return slot.Combatant;
            }
        }
    }

    public IReadOnlyList<Combats.ICombatant> GetMaterialized(Combats.ICombatant actor, ITargetSelectorContext context)
    {
        return GetIterator(context).Where(x => x != actor).ToArray();
    }
}