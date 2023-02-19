using System.Linq;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.GameScreens.Speech;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class TextEventStageItem : ICampaignStageItem
{
    private readonly string _eventSid;
    private readonly IEventCatalog _eventCatalog;
    private readonly LocationSid _location;

    public TextEventStageItem(string eventSid, LocationSid location, IEventCatalog eventCatalog)
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
            new SpeechScreenTransitionArgs(currentCampaign, dialogue, _location));
    }
}