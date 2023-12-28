using System;
using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Combats;

namespace Client.Core;

internal sealed class HeroState
{
    public HeroState(string classSid, IStatValue hitPoints, IEnumerable<CombatMovement> availableMovements)
    {
        ClassSid = classSid;
        HitPoints = hitPoints;
        AvailableMovements = availableMovements.ToArray();

        Equipments = ArraySegment<Equipment>.Empty;
        Perks = ArraySegment<IPerk>.Empty;
    }

    public IReadOnlyCollection<CombatMovement> AvailableMovements { get; }

    public string ClassSid { get; }
    public IReadOnlyList<Equipment> Equipments { get; }
    public IStatValue HitPoints { get; }
    public IList<IPerk> Perks { get; }

    public static HeroState Create(string classSid)
    {
        //TODO create herostate from factory
        return new HeroState(classSid, new StatValue(3), ArraySegment<CombatMovement>.Empty);
    }
}