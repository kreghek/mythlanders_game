using Client.Core.Campaigns;
using Client.GameScreens.TextDialogue;
using Client.ScreenManagement;
using Client.Core;
using System.Linq;

namespace Client.Assets.StageItems;

internal sealed class RestStageItem : ICampaignStageItem
{
    private readonly IEventCatalog _eventCatalog;

    public RestStageItem(IEventCatalog eventCatalog)
    {
        _eventCatalog = eventCatalog;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        const string REST_RESOURCE_FILE_SID = "rest";

        var restEvent = _eventCatalog.Events.Single(x => x.Sid == REST_RESOURCE_FILE_SID);
        var restDialogue = _eventCatalog.GetDialogue(restEvent.GetDialogSid());

        screenManager.ExecuteTransition(
        currentScreen,
        ScreenTransition.Event,
            new TextDialogueScreenTransitionArgs(currentCampaign, restDialogue, restEvent, currentCampaign.Location.Sid)
            { IsReward = IsGoalStage });
    }

    public bool IsGoalStage { get; }
}