using Client.Core;
using Client.Core.Dialogues;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.GameScreens.Speech
{
    internal sealed class DialogueContextFactory
    {
        private readonly Globe _globe;
        private readonly Player _player;
        private readonly DialogueEvent _currentDialogueEvent;
        private readonly IStoryPointCatalog _storyPointCatalog;

        public DialogueContextFactory(Globe globe, IStoryPointCatalog storyPointCatalog, Player player, DialogueEvent currentDialogueEvent)
        {
            _globe = globe;
            _storyPointCatalog = storyPointCatalog;
            _player = player;
            _currentDialogueEvent = currentDialogueEvent;
        }

        public IEventContext Create()
        {
            return new EventContext(_globe, _storyPointCatalog, _player, _currentDialogueEvent);
        }
    }
}