using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets;
using Client.Core.Heroes.Factories;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace Client.Core;

internal sealed class HeroState
{
    private static readonly IDictionary<string, IHeroFactory> _heroFactories = CreateHeroFactoryMap();

    private readonly IEnumerable<ICombatantStatusFactory> _builtStatuses;

    public HeroState(string classSid, IStatValue hitPoints, IEnumerable<ICombatantStat> combatStats,
        IEnumerable<CombatMovement> availableMovements,
        IEnumerable<ICombatantStatusFactory> builtStatuses)
    {
        ClassSid = classSid;
        HitPoints = hitPoints;
        CombatStats = combatStats;
        AvailableMovements = availableMovements.ToArray();
        _builtStatuses = builtStatuses.ToArray();

        Equipments = ArraySegment<Equipment>.Empty;
        Perks = ArraySegment<IPerk>.Empty;

        _combatantStatuses = new Dictionary<string, ICombatantStatusFactory>();
    }

    private readonly IDictionary<string, ICombatantStatusFactory> _combatantStatuses;

    public IReadOnlyCollection<CombatMovement> AvailableMovements { get; }

    public string ClassSid { get; }
    public IEnumerable<ICombatantStat> CombatStats { get; }
    public IReadOnlyList<Equipment> Equipments { get; }
    public IStatValue HitPoints { get; }
    public IList<IPerk> Perks { get; }

    public IReadOnlyCollection<ICombatantStatusFactory> StartUpCombatStatuses =>
        _builtStatuses.Union(_combatantStatuses.Values).ToArray();

    public static HeroState Create(string classSid)
    {
        return _heroFactories[classSid].Create();
    }

    private static IDictionary<string, IHeroFactory> CreateHeroFactoryMap()
    {
        var heroFactories = CatalogHelper.GetAllFactories<IHeroFactory>();

        return heroFactories.ToDictionary(x => x.GetType().Name[..^"HeroFactory".Length], x => x);
    }

    public void AddCombatStatus(string sid, ICombatantStatusFactory combatantStatusFactory)
    {
        _combatantStatuses[sid] = combatantStatusFactory;
    }

    public void RemoveCombatStatus(string sid)
    {
        _combatantStatuses.Remove(sid);
    }

    public void DisableToCampaigns()
    {
        AvailableToCampaigns = false;
    }
    
    public void EnableToCampaigns()
    {
        AvailableToCampaigns = true;
    }

    public bool AvailableToCampaigns { get; private set; } = true;
}