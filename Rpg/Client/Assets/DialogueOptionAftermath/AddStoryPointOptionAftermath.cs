using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.DialogueOptionAftermath
{
    internal class AddStoryPointOptionAftermath : IDialogueOptionAftermath
    {
        private readonly string _storyPointSid;

        public AddStoryPointOptionAftermath(string storyPointSid)
        {
            _storyPointSid = storyPointSid;
        }

        public void Apply(IEventContext dialogContext)
        {
            dialogContext.AddStoryPoint(_storyPointSid);
        }
    }
}