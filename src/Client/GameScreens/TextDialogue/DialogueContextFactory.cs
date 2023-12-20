using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.TextDialogue;

internal sealed class DialogueContextFactory : IDialogueContextFactory<ParagraphConditionContext, CampaignAftermathContext>
{
    private readonly DialogueEvent _currentDialogueEvent;
    private readonly HeroCampaign _campaign;
    private readonly IDialogueEnvironmentManager _environmentManager;
    private readonly Globe _globe;
    private readonly Player _player;
    private readonly IStoryPointCatalog _storyPointCatalog;

    public DialogueContextFactory(Globe globe, IStoryPointCatalog storyPointCatalog, Player player,
        IDialogueEnvironmentManager environmentManager,
        DialogueEvent currentDialogueEvent,
        HeroCampaign campaign)
    {
        _globe = globe;
        _storyPointCatalog = storyPointCatalog;
        _player = player;
        _environmentManager = environmentManager;
        _currentDialogueEvent = currentDialogueEvent;
        _campaign = campaign;
    }

    public CampaignAftermathContext CreateAftermathContext()
    {
        return new CampaignAftermathContext(_globe, _storyPointCatalog, _player, _currentDialogueEvent,
            _environmentManager, _campaign);
    }

    public ParagraphConditionContext CreateParagraphConditionContext()
    {
        return new ParagraphConditionContext(_player);
    }
}