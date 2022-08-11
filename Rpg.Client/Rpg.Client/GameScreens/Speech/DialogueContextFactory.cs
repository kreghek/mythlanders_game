using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.GameScreens.Speech
{
    internal sealed class DialogueContextFactory
    {
        private readonly Globe _globe;
        private readonly Player _player;
        private readonly IStoryPointCatalog _storyPointCatalog;

        public DialogueContextFactory(Globe globe, IStoryPointCatalog storyPointCatalog, Player player)
        {
            _globe = globe;
            _storyPointCatalog = storyPointCatalog;
            _player = player;
        }

        public IEventContext Create()
        {
            return new EventContext(_globe, _storyPointCatalog, _player);
        }
    }
}