using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.GameScreens.Speech
{
    internal sealed class DialogueContextFactory
    {
        private readonly Globe _globe;
        private readonly IStoryPointCatalog _storyPointCatalog;

        public DialogueContextFactory(Globe globe, IStoryPointCatalog storyPointCatalog)
        {
            _globe = globe;
            _storyPointCatalog = storyPointCatalog;
        }

        public IEventContext Create()
        {
            return new EventContext(_globe, _storyPointCatalog);
        }
    }
}