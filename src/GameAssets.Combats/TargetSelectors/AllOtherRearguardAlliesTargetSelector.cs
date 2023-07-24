namespace Core.Combats.TargetSelectors;

public sealed class AllOtherRearguardAlliesTargetSelector : ITargetSelector
{
    private static IEnumerable<Combats.ICombatant> GetIterator(Combats.ICombatant actor, ITargetSelectorContext context)
    {
        for (var lineIndex = 0; lineIndex < context.ActorSide.LineCount; lineIndex++)
        {
            var slot = context.ActorSide[new FieldCoords(1, lineIndex)];
            if (slot.Combatant is not null && slot.Combatant != actor)
            {
                yield return slot.Combatant;
            }
        }
    }

    public IReadOnlyList<Combats.ICombatant> GetMaterialized(Combats.ICombatant actor, ITargetSelectorContext context)
    {
        return GetIterator(actor, context).ToArray();
    }
}