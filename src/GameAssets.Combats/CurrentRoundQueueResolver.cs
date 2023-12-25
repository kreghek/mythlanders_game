using CombatDicesTeam.Combats;

namespace GameAssets.Combats;

public class CurrentRoundQueueResolver : IRoundQueueResolver
{
    public IReadOnlyList<ICombatant> GetCurrentRoundQueue(IReadOnlyCollection<ICombatant> combatants)
    {
        var orderedByResolve = combatants
            .Where(x => !x.IsDead)
            .OrderByDescending(x => x.Stats.Single(s => s.Type == CombatantStatTypes.Resolve).Value.Current)
            .ThenByDescending(x => x.IsPlayerControlled)
            .ToArray();

        return orderedByResolve;
    }
}