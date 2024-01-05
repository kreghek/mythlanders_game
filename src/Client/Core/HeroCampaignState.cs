using CombatDicesTeam.Combats;

namespace Client.Core;

internal sealed class HeroCampaignState
{
    public HeroCampaignState(HeroState state, FormationSlot formationSlot)
    {
        HitPoints = new HeroCampaignStatValue(state.HitPoints);
        FormationSlot = formationSlot;
    }

    public IStatValue HitPoints { get; }
    public FormationSlot FormationSlot { get; }
}
