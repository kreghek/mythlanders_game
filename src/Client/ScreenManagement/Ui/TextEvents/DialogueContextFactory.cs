using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Common.GlobeNotifications;

using CombatDicesTeam.Dialogues;

namespace Client.ScreenManagement.Ui.TextEvents;

internal sealed class
    DialogueContextFactory : IDialogueContextFactory<ParagraphConditionContext, CampaignAftermathContext>
{
    private readonly HeroCampaign _campaign;
    private readonly DialogueEvent _currentDialogueEvent;
    private readonly IDialogueEnvironmentManager _environmentManager;
    private readonly IEventContext _eventContext;
    private readonly IGlobeNotificationManager _globeNotificationManager;
    private readonly GlobeNotificationFactory _globeNotificationFactory;
    private readonly Globe _globe;
    private readonly Player _player;
    private readonly IStoryPointCatalog _storyPointCatalog;

    public DialogueContextFactory(Globe globe, IStoryPointCatalog storyPointCatalog, Player player,
        IDialogueEnvironmentManager environmentManager,
        DialogueEvent currentDialogueEvent,
        HeroCampaign campaign, IEventContext eventContext,
        IGlobeNotificationManager globeNotificationManager,
        GlobeNotificationFactory globeNotificationFactory)
    {
        _globe = globe;
        _storyPointCatalog = storyPointCatalog;
        _player = player;
        _environmentManager = environmentManager;
        _currentDialogueEvent = currentDialogueEvent;
        _campaign = campaign;
        _eventContext = eventContext;
        _globeNotificationManager = globeNotificationManager;
        _globeNotificationFactory = globeNotificationFactory;
    }

    public CampaignAftermathContext CreateAftermathContext()
    {
        return new CampaignAftermathContext(_globe, _storyPointCatalog, _player, _currentDialogueEvent,
            _environmentManager, _campaign, _eventContext, _globeNotificationManager, _globeNotificationFactory);
    }

    public ParagraphConditionContext CreateParagraphConditionContext()
    {
        return new ParagraphConditionContext(_player);
    }
}