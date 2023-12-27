using System;
using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Combats;

namespace Client.Core;

internal sealed class HeroState
{
    public HeroState(string classSid, IStatValue hitPoints, IEnumerable<CombatMovement> availableMovements)
    {
        this.ClassSid = classSid;
        this.HitPoints = hitPoints;
        this.AvailableMovements = availableMovements.ToArray();
        
        this.Equipments = ArraySegment<Equipment>.Empty;
        this.Perks = ArraySegment<IPerk>.Empty;
    }

    public string ClassSid { get; }
    public IStatValue HitPoints { get; }
    public IReadOnlyCollection<CombatMovement> AvailableMovements { get; }
    public IReadOnlyList<Equipment> Equipments { get; }
    public IList<IPerk> Perks { get; }

    public static HeroState Create(string classSid)
    {
        //TODO create herostate from factory
        return new HeroState(classSid, new StatValue(3), ArraySegment<CombatMovement>.Empty);
    }
}