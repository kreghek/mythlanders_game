using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.GameScreens.Speech;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class TextEventStageItem : ICampaignStageItem
{
    private readonly string _dialogueSid;
    private readonly GlobeNodeSid _location;
    private readonly IEventCatalog _eventCatalog;

    public TextEventStageItem(string dialogueSid, GlobeNodeSid location, IEventCatalog eventCatalog)
    {
        _dialogueSid = dialogueSid;
        _location = location;
        _eventCatalog = eventCatalog;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        var dialogue = _eventCatalog.GetDialogue(_dialogueSid);

        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Event, new SpeechScreenTransitionArgs
        {
            CurrentDialogue = dialogue,
            Location = _location
        });
    }
}
