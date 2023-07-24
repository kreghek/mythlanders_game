namespace Core.Combats.TargetSelectors;

public sealed class ClosestAllyInColumnTargetSelector : ITargetSelector
{
    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        var actorCoords = context.ActorSide.GetCombatantCoords(actor);

        var allyInColumn = context.ActorSide.GetColumnCombatants(actorCoords.ColumentIndex);

        var lastDist = 2;
        Combatant? closestAlly = null;

        foreach (var allyCombatant in allyInColumn)
        {
            if (allyCombatant == actor)
            {
                continue;
            }

            var allyCoords = context.ActorSide.GetCombatantCoords(allyCombatant);

            var colDist = Math.Abs(allyCoords.ColumentIndex - actorCoords.ColumentIndex);

            if (colDist < lastDist)
            {
                lastDist = colDist;
                closestAlly = allyCombatant;
            }
        }

        if (closestAlly is not null)
        {
            return new[]
            {
                closestAlly
            };
        }

        return ArraySegment<Combatant>.Empty;
    }
}