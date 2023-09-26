using CombatDicesTeam.Combats;

namespace Core.Combats.TargetSelectors;

public sealed class AllVanguardEnemiesTargetSelector : ITargetSelector
{
    private static IEnumerable<ICombatant> GetIterator(ITargetSelectorContext context)
    {
        for (var lineIndex = 0; lineIndex < context.EnemySide.LineCount; lineIndex++)
        {
            var slot = context.EnemySide[new FieldCoords(0, lineIndex)];
            if (slot.Combatant is not null)
            {
                yield return slot.Combatant;
            }
        }
    }

    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        return GetIterator(context).ToArray();
    }
}
