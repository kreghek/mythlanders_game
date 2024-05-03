using System;

using Client.Assets.Catalogs;
using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.CampaignReward.Ui;
using Client.GameScreens.Common.CampaignResult;
using Client.GameScreens.Common.Result;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;

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
    private readonly bool _isReward;

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

        _isReward = args.IsReward;
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

        var eventContext = new EventContext(globe, storyPointCatalog, campaignArgs.DialogueEvent);

        return new DialogueContextFactory(globe, storyPointCatalog, player, dialogueEnvironmentManager,
            campaignArgs.DialogueEvent, _currentCampaign,
            eventContext);
    }


    protected override void HandleDialogueEnd()
    {
        _globeProvider.Globe.Update(_dice, _eventCatalog);
        _dialogueEnvironmentManager.Clean();
        _globeProvider.StoreCurrentGlobe();

        if (!_isReward)
        {
            _coordinator.MakeCommonTransition(this, _currentCampaign);
        }
        else
        {
            var resultModal = new ResultModal(Game.Services.GetRequiredService<IUiContentStorage>(),
                ResolutionIndependentRenderer, ResultDecoration.Victory, _currentCampaign.ActualRewards,
                Game.Content.Load<Texture2D>("Sprites/Ui/VictoryFlags_41x205"),
                CreateDrawers());

            resultModal.Closed += (_, _) =>
            {
                foreach (var effect in _currentCampaign.ActualRewards)
                {
                    effect.Apply(_globeProvider.Globe);
                }

                _coordinator.MakeGoalStageTransition(this, _currentCampaign);
            };

            AddModal(resultModal, isLate: false);
        }
    }

    private ICampaignRewardImageDrawer[] CreateDrawers()
    {
        var uiContentStorage = Game.Services.GetRequiredService<IUiContentStorage>();
        return new ICampaignRewardImageDrawer[]
        {
            new PropCampaignRewardImageDrawer(Game.Content.Load<Texture2D>("Sprites/GameObjects/EquipmentIcons"),
                uiContentStorage.GetMainFont(),
                _globeProvider.Globe.Player.Inventory),
            new LocationCampaignRewardImageDrawer(Game.Content),
            new HeroCampaignRewardImageDrawer(Game.Content,
                Game.Services.GetRequiredService<ICombatantGraphicsCatalog>()),
            new GlobeEffectCampaignRewardImageDrawer(uiContentStorage.GetMainFont())
        };
    }
}