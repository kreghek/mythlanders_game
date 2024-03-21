using System;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Campaign;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;

using Microsoft.Extensions.DependencyInjection;

namespace Client.ScreenManagement;

internal abstract class CampaignTextEventScreenBase : TextEventScreenBase<ParagraphConditionContext, CampaignAftermathContext>
{
    private readonly DialogueContextFactory _dialogueContextFactory;
    
    private readonly HeroCampaign _currentCampaign;
    
    private readonly GlobeProvider _globeProvider;
    private readonly IEventCatalog _eventCatalog;
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    
    private readonly IDice _dice;
    
    protected CampaignTextEventScreenBase(MythlandersGame game, CampaignTextEventScreenArgsBase args) : base(game, args)
    {
        var globeProvider = game.Services.GetService<GlobeProvider>();
        var globe = globeProvider.Globe ?? throw new InvalidOperationException();
        var player = globe.Player ?? throw new InvalidOperationException();
        var storyPointCatalog = game.Services.GetRequiredService<IStoryPointCatalog>();
        var dialogueEnvironmentManager = game.Services.GetRequiredService<IDialogueEnvironmentManager>();
        
        _currentCampaign = args.Campaign;
        _globeProvider = globeProvider;
        _dialogueEnvironmentManager = dialogueEnvironmentManager;
        _eventCatalog = game.Services.GetRequiredService<IEventCatalog>();
        
        _dice = Game.Services.GetService<IDice>();
        
        _dialogueContextFactory =
            new DialogueContextFactory(globe, storyPointCatalog, player, dialogueEnvironmentManager,
                args.DialogueEvent, args.Campaign,
                new EventContext(globe, storyPointCatalog, player, args.DialogueEvent));
    }

    protected override IDialogueContextFactory<ParagraphConditionContext, CampaignAftermathContext>
        DialogueContextFactory => _dialogueContextFactory;

    
    protected override void HandleDialogueEnd()
    {
        _globeProvider.Globe.Update(_dice, _eventCatalog);
        _dialogueEnvironmentManager.Clean();
        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
            new CampaignScreenTransitionArguments(_currentCampaign));

        _globeProvider.StoreCurrentGlobe();
    }
}