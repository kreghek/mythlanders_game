using Client.GameScreens.SlidingPuzzles;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class SlidingPuzzlesStageItem : ICampaignStageItem
{
    private readonly IDice _dice;
    private readonly GlobeProvider _globeProvider;

    public SlidingPuzzlesStageItem(GlobeProvider globeProvider, IDice dice)
    {
        _globeProvider = globeProvider;
        _dice = dice;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.SlidingPuzzles,
            new SlidingPuzzlesScreenTransitionArguments(currentCampaign));
    }
}