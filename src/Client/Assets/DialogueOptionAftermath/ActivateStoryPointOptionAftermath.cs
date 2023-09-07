using Client.Core.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class ActivateStoryPointOptionAftermath : IDialogueOptionAftermath
{
    private readonly string _storyPointSid;

    public ActivateStoryPointOptionAftermath(string storyPointSid)
    {
        _storyPointSid = storyPointSid;
    }

    public void Apply(IEventContext dialogContext)
    {
        dialogContext.AddStoryPoint(_storyPointSid);
    }
}