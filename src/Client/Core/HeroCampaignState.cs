using System;
using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Combats;

namespace Client.Core;

internal sealed class HeroCampaignState
{
    public HeroCampaignState(HeroState state, FormationSlot formationSlot)
    {
        HitPoints = new HeroCampaignStatValue(state.HitPoints);
        CombatStats = state.CombatStats.ToArray();
        FormationSlot = formationSlot;
        AvailableMovements = state.AvailableMovements.ToArray();
        ClassSid = state.ClassSid;
        Equipments = state.Equipments;
        Perks = state.Perks;

        InitialStatuses = Array.Empty<IHeroCombatStatus>();
    }

    public IStatValue HitPoints { get; }
    public IEnumerable<ICombatantStat> CombatStats { get; }
    public FormationSlot FormationSlot { get; }

    public IReadOnlyCollection<CombatMovement> AvailableMovements { get; }

    public string ClassSid { get; }
    public IReadOnlyList<Equipment> Equipments { get; }
    public IList<IPerk> Perks { get; }

    public IReadOnlyCollection<IHeroCombatStatus> InitialStatuses { get; }
}
