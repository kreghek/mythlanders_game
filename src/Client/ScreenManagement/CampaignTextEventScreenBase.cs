using System;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;

using Microsoft.Extensions.DependencyInjection;

namespace Client.ScreenManagement;

internal abstract class
    CampaignTextEventScreenBase : TextEventScreenBase<ParagraphConditionContext, CampaignAftermathContext>
{
    private readonly StateCoordinator _coordinator;
    private readonly HeroCampaign _currentCampaign;
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;

    private readonly IDice _dice;
    private readonly IEventCatalog _eventCatalog;

    private readonly GlobeProvider _globeProvider;

    protected CampaignTextEventScreenBase(MythlandersGame game, CampaignTextEventScreenArgsBase args) : base(game, args)
    {
        var globeProvider = game.Services.GetService<GlobeProvider>();

        var dialogueEnvironmentManager = game.Services.GetRequiredService<IDialogueEnvironmentManager>();

        _currentCampaign = args.Campaign;
        _globeProvider = globeProvider;
        _dialogueEnvironmentManager = dialogueEnvironmentManager;
        _eventCatalog = game.Services.GetRequiredService<IEventCatalog>();
        _coordinator = game.Services.GetRequiredService<StateCoordinator>();

        _dice = Game.Services.GetService<IDice>();
    }

    protected override IDialogueContextFactory<ParagraphConditionContext, CampaignAftermathContext>
        CreateDialogueContextFactory(TextEventScreenArgsBase<ParagraphConditionContext, CampaignAftermathContext> args)
    {
        var globeProvider = Game.Services.GetService<GlobeProvider>();
        var globe = globeProvider.Globe ?? throw new InvalidOperationException();
        var player = globe.Player ?? throw new InvalidOperationException();

        var storyPointCatalog = Game.Services.GetRequiredService<IStoryPointCatalog>();

        var dialogueEnvironmentManager = Game.Services.GetRequiredService<IDialogueEnvironmentManager>();

        var campaignArgs = (CampaignTextEventScreenArgsBase)args;

        return new DialogueContextFactory(globe, storyPointCatalog, player, dialogueEnvironmentManager,
            campaignArgs.DialogueEvent, _currentCampaign,
            new EventContext(globe, storyPointCatalog, player, campaignArgs.DialogueEvent));
    }


    protected override void HandleDialogueEnd()
    {
        _globeProvider.Globe.Update(_dice, _eventCatalog);
        _dialogueEnvironmentManager.Clean();
        _globeProvider.StoreCurrentGlobe();

        _coordinator.MakeCommonTransition(this, _currentCampaign);
    }
}