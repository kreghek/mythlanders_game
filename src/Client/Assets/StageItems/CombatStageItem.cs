using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Combat;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class CombatStageItem : ICampaignStageItem
{
    private readonly GlobeNode _location;

    public CombatStageItem(GlobeNode location, CombatSequence combatSequence)
    {
        _location = location;
        CombatSequence = combatSequence;
    }

    internal CombatSequence CombatSequence { get; }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Combat,
            new CombatScreenTransitionArguments(currentCampaign, CombatSequence, 0, false, _location, null));
    }
}