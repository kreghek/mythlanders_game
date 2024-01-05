using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.Heroes.Factories;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace Client.Core;

internal sealed class HeroState
{
    private static readonly IDictionary<string, IHeroFactory> _heroFactories =
        new Dictionary<string, IHeroFactory>
        {
            { "swordsman", new SwordsmanHeroFactory() },
            { "amazon", new AmazonHeroFactory() },
            { "partisan", new PartisanHeroFactory() },
            { "robber", new RobberHeroFactory() },
            { "monk", new MonkHeroFactory() },
            { "guardian", new GuardianHeroFactory() }
        };

    public HeroState(string classSid, IStatValue hitPoints, IEnumerable<ICombatantStat> combatStats,
        IEnumerable<CombatMovement> availableMovements,
        IReadOnlyCollection<ICombatantStatusFactory> startUpCombatStatuses)
    {
        ClassSid = classSid;
        HitPoints = hitPoints;
        CombatStats = combatStats;
        AvailableMovements = availableMovements.ToArray();
        StartUpCombatStatuses = startUpCombatStatuses.ToArray();

        Equipments = ArraySegment<Equipment>.Empty;
        Perks = ArraySegment<IPerk>.Empty;
    }

    public IReadOnlyCollection<CombatMovement> AvailableMovements { get; }

    public string ClassSid { get; }
    public IEnumerable<ICombatantStat> CombatStats { get; }
    public IReadOnlyList<Equipment> Equipments { get; }
    public IStatValue HitPoints { get; }
    public IList<IPerk> Perks { get; }

    public IReadOnlyCollection<ICombatantStatusFactory> StartUpCombatStatuses { get; }

    public static HeroState Create(string classSid)
    {
        return _heroFactories[classSid].Create();
    }
}