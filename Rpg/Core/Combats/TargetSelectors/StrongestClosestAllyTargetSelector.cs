namespace Core.Combats.TargetSelectors;

public sealed class StrongestClosestAllyTargetSelector : ITargetSelector
{
    private static List<(FieldCoords coords, int priority)> GetNeighbourCoords(FieldCoords actorCoords)
    {
        var coordList = new List<(FieldCoords coords, int priority)>();
        if (actorCoords.LineIndex > 0)
        {
            coordList.Add((new FieldCoords(actorCoords.ColumentIndex, actorCoords.LineIndex - 1), 1));
            coordList.Add((new FieldCoords(actorCoords.ColumentIndex == 0 ? 1 : 0, actorCoords.LineIndex - 1), 0));
        }

        if (actorCoords.LineIndex < 2)
        {
            coordList.Add((new FieldCoords(actorCoords.ColumentIndex, actorCoords.LineIndex + 1), 1));
            coordList.Add((new FieldCoords(actorCoords.ColumentIndex == 0 ? 1 : 0, actorCoords.LineIndex + 1), 0));
        }

        coordList.Add((new FieldCoords(actorCoords.ColumentIndex == 0 ? 1 : 0, actorCoords.LineIndex), 1));
        return coordList;
    }

    public IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context)
    {
        var actorSide = context.ActorSide;

        var actorCoords = actorSide.GetCombatantCoords(actor);

        var actorLine = actorCoords.LineIndex;
        var coordList = GetNeighbourCoords(actorCoords);

        var prioritySorted = coordList.OrderBy(x => x.priority).ToArray();

        var neighbourCombatants = new List<(Combatant combatant, int proprity)>();
        foreach (var (coords, priority) in prioritySorted)
        {
            var slot = actorSide[coords];

            if (slot.Combatant is not null) neighbourCombatants.Add((slot.Combatant, priority));
        }

        var sortedCombatants = neighbourCombatants.OrderByDescending(x => x.proprity)
            .ThenByDescending(x => x.combatant.Stats.Single(s => s.Type == UnitStatType.HitPoints).Value.Current)
            .Select(x => x.combatant).ToArray();

        if (!sortedCombatants.Any()) return Array.Empty<Combatant>();

        return new[]
        {
            sortedCombatants.First()
        };
    }
}