using CombatDicesTeam.Combats;

namespace Core.Combats.TargetSelectors;

public sealed class AllLineEnemiesTargetSelector : ITargetSelector
{
    private static IEnumerable<ICombatant> GetIterator(ICombatant actor, ITargetSelectorContext context)
    {
        var attackerLine = GetActorLine(actor, context);

        for (var columnIndex = 0; columnIndex < context.EnemySide.ColumnCount; columnIndex++)
        {
            var slot = context.EnemySide[new FieldCoords(columnIndex, attackerLine)];
            if (slot.Combatant is not null)
            {
                yield return slot.Combatant;
            }
        }
    }

    private static int GetActorLine(ICombatant actor, ITargetSelectorContext context)
    {
        var actorLine = 0;

        for (var columnIndex = 0; columnIndex < context.ActorSide.ColumnCount; columnIndex++)
            for (var lineIndex = 0; lineIndex < context.ActorSide.LineCount; lineIndex++)
            {
                var fieldCoords = new FieldCoords(columnIndex, lineIndex);
                if (context.ActorSide[fieldCoords].Combatant == actor)
                {
                    actorLine = lineIndex;
                }
            }

        return actorLine;
    }

    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        return GetIterator(actor, context).ToArray();
    }
}