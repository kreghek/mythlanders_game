using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs;
using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Bestiary;
using Client.GameScreens.Campaign.Tutorial;
using Client.GameScreens.Campaign.Ui;
using Client.GameScreens.Common;
using Client.ScreenManagement;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.GameScreens.Campaign;

internal class CampaignScreen : GameScreenWithMenuBase
{
    private readonly ButtonBase _bestiaryButton;
    private readonly HeroCampaign _currentCampaign;
    private readonly GlobeProvider _globeProvider;
    private readonly ButtonBase _inventoryButton;
    private readonly VerticalStackPanel _jobElement;
    private readonly CampaignScreenTransitionArguments _screenTransitionArguments;
    private readonly ButtonBase _showQuestsPanelButton;
    private readonly IUiContentStorage _uiContentStorage;
    private CampaignEffectsPanel? _campaignEffectsPanel;
    private CampaignMap? _campaignMap;

    private bool _isCampaignPresentation = true;

    private double _presentationDelayCounter = 3;

    private bool _showQuests;

    public CampaignScreen(MythlandersGame game, CampaignScreenTransitionArguments screenTransitionArguments) :
        base(game)
    {
        _screenTransitionArguments = screenTransitionArguments;
        _currentCampaign = screenTransitionArguments.Campaign;

        _globeProvider = game.Services.GetRequiredService<GlobeProvider>();
        _uiContentStorage = game.Services.GetRequiredService<IUiContentStorage>();

        _showQuestsPanelButton = new ResourceTextButton(nameof(UiResource.CurrentQuestButtonTitle));
        _showQuestsPanelButton.OnClick += ShowStoryPointsButton_OnClick;

        _inventoryButton = new ResourceTextButton(nameof(UiResource.InventoryButtonTitle));
        _inventoryButton.OnClick += InventoryButton_OnClick;

        _bestiaryButton = new ResourceTextButton(nameof(UiResource.BestiaryButtonTitle));
        _bestiaryButton.OnClick += BestiaryButton_OnClick;

        var executables = _globeProvider.Globe.GetCurrentJobExecutables().OfType<IDisplayableJobExecutable>()
            .OrderBy(x => x.Order).ThenBy(x => x.TitleSid).ToArray();
        var executablesElements = CreateJobElements(executables);

        _jobElement = new VerticalStackPanel(_uiContentStorage.GetControlBackgroundTexture(), ControlTextures.Panel,
            executablesElements);
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        var menuButtons = new List<ButtonBase>();

        if (_globeProvider.Globe.Player.Inventory.CalcActualItems().Any())
        {
            menuButtons.Add(_inventoryButton);
        }

        if (_globeProvider.Globe.Player.MonsterPerks.Any() && _globeProvider.Globe.Player.KnownMonsters.Any())
        {
            menuButtons.Add(_bestiaryButton);
        }

        return menuButtons;
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        ResolutionIndependentRenderer.BeginDraw();
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        if (_campaignMap is not null)
        {
            _campaignMap.Rect = contentRect;
            _campaignMap.Draw(spriteBatch);

            if (_isCampaignPresentation)
            {
                spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), UiResource.SkipMapPresentationHintText,
                    new Vector2(contentRect.Center.X, contentRect.Bottom - 50), Color.Wheat);
            }
        }

        const int STORY_POINT_PANEL_WIDTH = 200;
        const int STORY_POINT_PANEL_HEIGHT = 400;
        var storyPointRect = new Rectangle(
            contentRect.Right - STORY_POINT_PANEL_WIDTH - ControlBase.CONTENT_MARGIN,
            contentRect.Top + ControlBase.CONTENT_MARGIN,
            STORY_POINT_PANEL_WIDTH,
            STORY_POINT_PANEL_HEIGHT);

        DrawCurrentStoryPoints(spriteBatch, storyPointRect);

        var campaignEffectRect = new Rectangle(100 + contentRect.Left, contentRect.Top, 100, contentRect.Height);
        DrawCampaignEffects(spriteBatch, campaignEffectRect);

        DrawHeroes(spriteBatch, contentRect);

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        SaveGameProgress();
        InitializeCampaignItemButtons();
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);
        
        CheckTutorial();

        if (_campaignMap is not null)
        {
            _campaignMap.Update(gameTime, ResolutionIndependentRenderer);

            UpdateMapPresentation(gameTime, _campaignMap);
        }

        if (_globeProvider.Globe.Features.HasFeature(GameFeatures.ExecutableQuests))
        {
            _showQuestsPanelButton.Update(ResolutionIndependentRenderer);

            if (_showQuests)
            {
                _showQuestsPanelButton.Update(ResolutionIndependentRenderer);
            }
        }
    }
    
    private void CheckTutorial()
    {
        if (_globeProvider.Globe.Player.HasAbility(PlayerAbility.SkipTutorials))
        {
            return;
        }

        if (!_globeProvider.Globe.Player.HasAbility(PlayerAbility.ReadSideQuestTutorial))
        {
            _globeProvider.Globe.Player.AddPlayerAbility(PlayerAbility.ReadSideQuestTutorial);

            var tutorialModal = new TutorialModal(new CampaignMapTutorialPageDrawer(_uiContentStorage), _uiContentStorage,
                ResolutionIndependentRenderer, _globeProvider.Globe.Player);
            AddModal(tutorialModal, isLate: false);
        }
    }

    private void BestiaryButton_OnClick(object? sender, EventArgs e)
    {
        ScreenManager.ExecuteTransition(this, ScreenTransition.Bestiary,
            new BestiaryScreenTransitionArguments(ScreenTransition.Campaign,
                new CampaignScreenTransitionArguments(_currentCampaign)));
    }

    private IReadOnlyList<ControlBase> CreateJobElements(IDisplayableJobExecutable[] executables)
    {
        var executableList = new List<ControlBase>();

        foreach (var executable in executables)
        {
            var jobTextList = new List<ControlBase>();

            var storyPoint = executable;

            jobTextList.Add(new Text(
                _uiContentStorage.GetControlBackgroundTexture(),
                ControlTextures.Panel,
                _uiContentStorage.GetTitlesFont(),
                _ => Color.White,
                () => storyPoint.TitleSid));

            if (storyPoint.CurrentJobs is not null)
            {
                var currentJobs = storyPoint.CurrentJobs.ToList();
                foreach (var job in currentJobs)
                {
                    var jobClosure = job;
                    jobTextList.Add(new Text(
                        _uiContentStorage.GetControlBackgroundTexture(),
                        ControlTextures.Panel,
                        _uiContentStorage.GetMainFont(),
                        _ => Color.Wheat,
                        () => jobClosure.ToString() ?? string.Empty));
                }
            }

            var executablePanel = new VerticalStackPanel(_uiContentStorage.GetControlBackgroundTexture(),
                ControlTextures.Transparent, jobTextList);
            executableList.Add(executablePanel);
        }

        return executableList;
    }

    private void DrawCampaignEffects(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        if (_globeProvider.Globe.Features.HasFeature(GameFeatures.CampaignEffects) && _campaignEffectsPanel is not null)
        {
            _campaignEffectsPanel.Rect = new Rectangle(
                contentRect.Left + ControlBase.CONTENT_MARGIN,
                contentRect.Top + ControlBase.CONTENT_MARGIN,
                200,
                ControlBase.CONTENT_MARGIN * 5 + 20 * 4);

            _campaignEffectsPanel.Draw(spriteBatch);
        }
    }

    private void DrawCurrentStoryPoints(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        if (!_globeProvider.Globe.GetCurrentJobExecutables().OfType<IDisplayableJobExecutable>().Any())
        {
            return;
        }

        if (_globeProvider.Globe.Features.HasFeature(GameFeatures.ExecutableQuests))
        {
            _showQuestsPanelButton.Rect = new Rectangle(contentRect.Right - 50, contentRect.Top, 50, 20);
            _showQuestsPanelButton.Draw(spriteBatch);

            if (_showQuests)
            {
                _jobElement.Rect = contentRect;
                _jobElement.Draw(spriteBatch);
            }
        }
    }

    private void DrawHeroes(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        var heroes = _currentCampaign.Heroes.ToArray();
        for (var i = 0; i < heroes.Length; i++)
        {
            var hero = heroes[i];
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                $"{hero.ClassSid} {hero.HitPoints.Current}/{hero.HitPoints.ActualMax}",
                new Vector2(contentRect.Left, contentRect.Top + i * 20), MythlandersColors.MainSciFi);
        }
    }

    private void HandleSkipPresentation(CampaignMap campaignMap)
    {
        if (!Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            return;
        }

        if (campaignMap.Presentation is null)
        {
            // Presentation data is not ready yet.
            return;
        }

        _presentationDelayCounter = 0;
        _isCampaignPresentation = false;
        campaignMap.State = CampaignMap.MapState.Interactive;

        campaignMap.Scroll = campaignMap.Presentation.Target;
    }

    private void InitializeCampaignItemButtons()
    {
        var currentCampaign = _screenTransitionArguments.Campaign;

        InitializeCampaignMap(currentCampaign);

        InitializeCampaignEffectPanel(currentCampaign);
    }

    private void InitializeCampaignMap(HeroCampaign currentCampaign)
    {
        _campaignMap = new CampaignMap(currentCampaign, ScreenManager, this,
            Game.Content.Load<Texture2D>("Sprites/Ui/CampaignStageIcons"),
            Game.Content.Load<Texture2D>("Sprites/Ui/MapBackground"),
            Game.Content.Load<Texture2D>("Sprites/Ui/MapItemShadow"),
            Game.Content.Load<Texture2D>("Sprites/Ui/MapDisplay"),
            Game.Content.Load<Texture2D>("Sprites/Ui/Icons16x16"),
            ResolutionIndependentRenderer,
            Game.Services.GetRequiredService<GameObjectContentStorage>(),
            Game.Services.GetRequiredService<ICombatantGraphicsCatalog>());
    }

    private void InitializeCampaignEffectPanel(HeroCampaign currentCampaign)
    {
        if (_globeProvider.Globe.Features.HasFeature(GameFeatures.CampaignEffects))
        {
            var rewards = _screenTransitionArguments.Campaign.ActualRewards.ToArray();
            var penalties = currentCampaign.ActualFailurePenalties;

            if (rewards.Any() || penalties.Any())
            {
                _campaignEffectsPanel = new CampaignEffectsPanel(rewards, penalties);
            }
        }
    }

    private void InventoryButton_OnClick(object? sender, EventArgs e)
    {
        AddModal(
            new InventoryModal(_globeProvider.Globe.Player.Inventory,
                Game.Services.GetRequiredService<IUiContentStorage>(),
                ResolutionIndependentRenderer), false);
    }

    private void SaveGameProgress()
    {
        _globeProvider.StoreCurrentGlobe();
    }

    private void ShowStoryPointsButton_OnClick(object? sender, EventArgs e)
    {
        _showQuests = !_showQuests;
    }

    private void UpdateMapPresentation(GameTime gameTime, CampaignMap campaignMap)
    {
        if (!_isCampaignPresentation)
        {
            return;
        }

        HandleSkipPresentation(campaignMap);

        if (_presentationDelayCounter > 0)
        {
            _presentationDelayCounter -= gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            if (campaignMap.Presentation is null)
            {
                // Presentation is not ready yet.
                return;
            }

            if ((campaignMap.Scroll - campaignMap.Presentation.Target).Length() > 10)
            {
                campaignMap.Scroll = Vector2.Lerp(campaignMap.Scroll, campaignMap.Presentation.Target,
                    (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f);
            }
            else
            {
                _isCampaignPresentation = false;
                campaignMap.State = CampaignMap.MapState.Interactive;
            }
        }
    }
}