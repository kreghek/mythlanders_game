using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.GameScreens.Speech;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.Assets.StageItems
{
    internal sealed class DialogueStageItem : ICampaignStageItem
    {
        private readonly GlobeNode _location;
        private readonly Dialogue _dialogue;

        public DialogueStageItem(GlobeNode location, Dialogue dialogue)
        {
            _location = location;
            _dialogue = dialogue;
        }

        public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager)
        {
            screenManager.ExecuteTransition(currentScreen, ScreenTransition.Event, new SpeechScreenTransitionArgs
            {
                CurrentDialogue = _dialogue,
                Location = _location
            });
        }
    }
}