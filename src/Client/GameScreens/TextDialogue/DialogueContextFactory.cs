using Client.Assets.Dialogues;
using Client.Core;
using Client.Core.Dialogues;

namespace Client.GameScreens.TextDialogue;

internal sealed class DialogueContextFactory: IDialogueContextFactory<DialogueParagraphConditionContext>
{
    private readonly DialogueEvent _currentDialogueEvent;
    private readonly Globe _globe;
    private readonly Player _player;
    private readonly IStoryPointCatalog _storyPointCatalog;

    public DialogueContextFactory(Globe globe, IStoryPointCatalog storyPointCatalog, Player player,
        DialogueEvent currentDialogueEvent)
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