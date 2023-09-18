using Client.Assets.Catalogs.Dialogues;
using Client.Assets.Dialogues;
using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.TextDialogue;

internal sealed class DialogueContextFactory: IDialogueContextFactory<ParagraphConditionContext, AftermathContext>
{
    private readonly DialogueEvent _currentDialogueEvent;
    private readonly Globe _globe;
    private readonly Player _player;
    private readonly IDialogueEnvironmentManager _environmentManager;
    private readonly IStoryPointCatalog _storyPointCatalog;

    public DialogueContextFactory(Globe globe, IStoryPointCatalog storyPointCatalog, Player player, IDialogueEnvironmentManager environmentManager,
        DialogueEvent currentDialogueEvent)
    {
        _globe = globe;
        _storyPointCatalog = storyPointCatalog;
        _player = player;
        _environmentManager = environmentManager;
        _currentDialogueEvent = currentDialogueEvent;
    }

    public AftermathContext CreateAftermathContext()
    {
        return new AftermathContext(_globe, _storyPointCatalog, _player, _currentDialogueEvent, _environmentManager);
    }

    public ParagraphConditionContext CreateParagraphConditionContext()
    {
        return new ParagraphConditionContext(_player);
    }
}