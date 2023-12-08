using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Engine;
using Client.GameScreens.Campaign.Ui;
using Client.ScreenManagement;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.GameScreens.Campaign;

internal class CampaignScreen : GameScreenWithMenuBase
{
    private readonly GlobeProvider _globe;
    private readonly ButtonBase _inventoryButton;
    private readonly CampaignScreenTransitionArguments _screenTransitionArguments;
    private readonly ButtonBase _showStoryPointsButton;
    private readonly IUiContentStorage _uiContentStorage;
    private CampaignEffectsPanel _campaignEffectsPanel = null!;
    private CampaignMap? _campaignMap;

    private bool _isCampaignPresentation = true;

    private double _presentationDelayCounter = 3;

    private bool _showStoryPoints;

    public CampaignScreen(TestamentGame game, CampaignScreenTransitionArguments screenTransitionArguments) : base(game)
    {
        _screenTransitionArguments = screenTransitionArguments;

        _globe = game.Services.GetRequiredService<GlobeProvider>();
        _uiContentStorage = game.Services.GetRequiredService<IUiContentStorage>();

        _showStoryPointsButton = new ResourceTextButton(nameof(UiResource.CurrentQuestButtonTitle));
        _showStoryPointsButton.OnClick += ShowStoryPointsButton_OnClick;

        _inventoryButton = new ResourceTextButton(nameof(UiResource.InventoryButtonTitle));
        _inventoryButton.OnClick += InventoryButton_OnClick;
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        if (_globe.Globe.Player.Inventory.CalcActualItems().Any())
        {
            return new[]
            {
                _inventoryButton
            };
        }

        return Array.Empty<ButtonBase>();
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

        DrawCampaignEffects(spriteBatch, contentRect);

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
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

        _showStoryPointsButton.Update(ResolutionIndependentRenderer);
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
        if (!_globe.Globe.ActiveStoryPoints.Any() && _globe.Globe.Player.Challenge is null)
        {
            return;
        }

        _showStoryPointsButton.Rect = new Rectangle(contentRect.Right - 50, contentRect.Top, 50, 20);
        _showStoryPointsButton.Draw(spriteBatch);

        if (_showStoryPoints)
        {
            var storyPoints = _globe.Globe.ActiveStoryPoints.OrderBy(x => x.Sid).ToArray();
            for (var storyPointIndex = 0; storyPointIndex < storyPoints.Length; storyPointIndex++)
            {
                var storyPoint = storyPoints[storyPointIndex];
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

            if (_globe.Globe.Player.Challenge is not null)
            {
                var challengeStartY = contentRect.Top + storyPoints.Length * 20 + 20;

                spriteBatch.DrawString(
                    UiThemeManager.UiContentStorage.GetMainFont(),
                    UiResource.CampaignStageDisplayNameChallenge,
                    new Vector2(contentRect.Left, challengeStartY),
                    Color.Wheat);

                var challengeJobs = _globe.Globe.Player.Challenge.CurrentJobs;
                if (challengeJobs is not null)
                {
                    var currentJobs = challengeJobs.ToArray();
                    for (var jobNumber = 0; jobNumber < currentJobs.Length; jobNumber++)
                    {
                        var job = currentJobs[jobNumber];
                        var jobTextOffsetY = challengeStartY + 20 * jobNumber;
                        spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetMainFont(),
                            currentJobs[jobNumber].ToString(),
                            new Vector2(contentRect.Left, contentRect.Top + 20 + jobTextOffsetY + 20),
                            Color.Wheat);
                    }
                }
            }
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
            new InventoryModal(_globe.Globe.Player.Inventory, Game.Services.GetRequiredService<IUiContentStorage>(),
                ResolutionIndependentRenderer), false);
    }

    private void ShowStoryPointsButton_OnClick(object? sender, EventArgs e)
    {
        _showStoryPoints = !_showStoryPoints;
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