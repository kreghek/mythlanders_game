using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

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

        StartUpCombatStatuses = state.StartUpCombatStatuses.ToArray();
    }

    public IReadOnlyCollection<CombatMovement> AvailableMovements { get; }

    public string ClassSid { get; }
    public IEnumerable<ICombatantStat> CombatStats { get; }
    public IReadOnlyList<Equipment> Equipments { get; }
    public FormationSlot FormationSlot { get; }

    public IStatValue HitPoints { get; }
    public IList<IPerk> Perks { get; }

    public IReadOnlyCollection<ICombatantStatusFactory> StartUpCombatStatuses { get; }
}