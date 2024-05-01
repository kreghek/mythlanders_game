using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Bestiary;
using Client.GameScreens.Campaign.Ui;
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
    private readonly CampaignScreenTransitionArguments _screenTransitionArguments;
    private readonly ButtonBase _showQuestsPanelButton;
    private readonly IUiContentStorage _uiContentStorage;
    private CampaignEffectsPanel _campaignEffectsPanel = null!;
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

        if (_campaignMap is not null)
        {
            _campaignMap.Update(gameTime, ResolutionIndependentRenderer);

            UpdateMapPresentation(gameTime, _campaignMap);
        }

        _showQuestsPanelButton.Update(ResolutionIndependentRenderer);
    }

    private void BestiaryButton_OnClick(object? sender, EventArgs e)
    {
        ScreenManager.ExecuteTransition(this, ScreenTransition.Bestiary,
            new BestiaryScreenTransitionArguments(ScreenTransition.Campaign,
                new CampaignScreenTransitionArguments(_currentCampaign)));
    }

    private void DrawCampaignEffects(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        _campaignEffectsPanel.Rect = new Rectangle(
            contentRect.Left + ControlBase.CONTENT_MARGIN,
            contentRect.Top + ControlBase.CONTENT_MARGIN,
            200,
            ControlBase.CONTENT_MARGIN * 5 + 20 * 4);

        _campaignEffectsPanel.Draw(spriteBatch);
    }

    private void DrawCurrentStoryPoints(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        if (!_globeProvider.Globe.GetCurrentJobExecutables().OfType<IDisplayableJobExecutable>().Any())
        {
            return;
        }

        _showQuestsPanelButton.Rect = new Rectangle(contentRect.Right - 50, contentRect.Top, 50, 20);
        _showQuestsPanelButton.Draw(spriteBatch);

        if (_showQuests)
        {
            var executables = _globeProvider.Globe.GetCurrentJobExecutables().OfType<IDisplayableJobExecutable>()
                .OrderBy(x => x.Order).ThenBy(x => x.TitleSid).ToArray();
            for (var storyPointIndex = 0; storyPointIndex < executables.Length; storyPointIndex++)
            {
                var storyPoint = executables[storyPointIndex];
                spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetMainFont(), storyPoint.TitleSid,
                    new Vector2(contentRect.Left, contentRect.Top + storyPointIndex * 20), Color.Wheat);
                if (storyPoint.CurrentJobs is not null)
                {
                    var currentJobs = storyPoint.CurrentJobs.ToList();
                    for (var jobNumber = 0; jobNumber < currentJobs.Count; jobNumber++)
                    {
                        var jobTextOffsetY = 20 * jobNumber;
                        spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetMainFont(),
                            currentJobs[jobNumber].ToString(),
                            new Vector2(contentRect.Left, contentRect.Top + 20 + jobTextOffsetY),
                            Color.Wheat);
                    }
                }
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

        _campaignMap = new CampaignMap(currentCampaign, ScreenManager, this,
            Game.Content.Load<Texture2D>("Sprites/Ui/CampaignStageIcons"),
            Game.Content.Load<Texture2D>("Sprites/Ui/MapBackground"),
            Game.Content.Load<Texture2D>("Sprites/Ui/MapItemShadow"),
            Game.Content.Load<Texture2D>("Sprites/Ui/MapDisplay"),
            Game.Content.Load<Texture2D>("Sprites/Ui/Icons16x16"),
            ResolutionIndependentRenderer,
            Game.Services.GetRequiredService<GameObjectContentStorage>());

        var rewards = _screenTransitionArguments.Campaign.ActualRewards.ToArray();
        _campaignEffectsPanel = new CampaignEffectsPanel(rewards, currentCampaign.ActualFailurePenalties);
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