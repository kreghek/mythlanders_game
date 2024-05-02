using System.Linq;

using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.TextDialogue;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class DialogueEventStageItem : ICampaignStageItem
{
    private readonly IEventCatalog _eventCatalog;
    private readonly string _eventSid;
    private readonly ILocationSid _location;

    public DialogueEventStageItem(string eventSid, ILocationSid location, IEventCatalog eventCatalog)
    {
        _eventSid = eventSid;
        _location = location;
        _eventCatalog = eventCatalog;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        var textEvent = _eventCatalog.Events.Single(x => x.Sid == _eventSid);

        var dialogueSid = textEvent.GetDialogSid();

        var dialogue = _eventCatalog.GetDialogue(dialogueSid);

        screenManager.ExecuteTransition(
            currentScreen,
            ScreenTransition.Event,
            new TextDialogueScreenTransitionArgs(currentCampaign, dialogue, textEvent, _location){ IsReward = IsReward});
    }

    public bool IsReward { get; init; }
}