namespace Core.Combats.TargetSelectors;

public sealed class AllOtherVanguardAlliesTargetSelector : ITargetSelector
{
    private static IEnumerable<Combatant> GetIterator(Combatant actor, ITargetSelectorContext context)
    {
        for (var lineIndex = 0; lineIndex < context.ActorSide.LineCount; lineIndex++)
        {
            var slot = context.ActorSide[new FieldCoords(0, lineIndex)];
            if (slot.Combatant is not null && slot.Combatant != actor)
            {
                yield return slot.Combatant;
            }
        }
    }

    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        return GetIterator(actor, context).ToArray();
    }
}