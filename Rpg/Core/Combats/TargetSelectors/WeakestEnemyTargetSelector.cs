namespace Core.Combats.TargetSelectors;

public sealed class WeakestEnemyTargetSelector : ITargetSelector
{
    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        var actorLine = 0;

        for (var columnIndex = 0; columnIndex < context.ActorSide.ColumnCount; columnIndex++)
        for (var lineIndex = 0; lineIndex < context.ActorSide.LineCount; lineIndex++)
        {
            var fieldCoords = new FieldCoords(columnIndex, lineIndex);
            if (context.ActorSide[fieldCoords].Combatant == actor) actorLine = lineIndex;
        }

        var vanguardCoords = new FieldCoords(0, actorLine);
        var closestEnemySlot = context.EnemySide[vanguardCoords];
        if (closestEnemySlot.Combatant is null)
        {
            var rearCoords = new FieldCoords(1, actorLine);
            closestEnemySlot = context.EnemySide[rearCoords];
        }

        if (closestEnemySlot.Combatant is null) return ArraySegment<Combatant>.Empty;

        return new[]
        {
            closestEnemySlot.Combatant
        };
    }
}