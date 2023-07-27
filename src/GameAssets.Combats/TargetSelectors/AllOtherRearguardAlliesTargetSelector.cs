using CombatDicesTeam.Combats;

namespace Core.Combats.TargetSelectors;

public sealed class AllOtherRearguardAlliesTargetSelector : ITargetSelector
{
    private static IEnumerable<ICombatant> GetIterator(ICombatant actor, ITargetSelectorContext context)
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

    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        return GetIterator(actor, context).ToArray();
    }
}