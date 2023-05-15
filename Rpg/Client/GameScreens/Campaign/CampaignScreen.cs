using System;
using System.Collections.Generic;
using System.Linq;

using Client.Engine;
using Client.GameScreens.Campaign.Ui;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Campaign;

internal class CampaignScreen : GameScreenWithMenuBase
{
    private readonly GlobeProvider _globe;
    private readonly ButtonBase _inventoryButton;
    private readonly CampaignScreenTransitionArguments _screenTransitionArguments;
    private readonly ButtonBase _showStoryPointsButton;
    private CampaignMap? _campaignMap;

    private bool _isCampaignPresentation = true;

    private double _presentationDelayCounter = 3;

    private bool _showStoryPoints;

    public CampaignScreen(TestamentGame game, CampaignScreenTransitionArguments screenTransitionArguments) : base(game)
    {
        _screenTransitionArguments = screenTransitionArguments;

        _globe = game.Services.GetRequiredService<GlobeProvider>();

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
        }

        const int STORY_POINT_PANEL_WIDTH = 200;
        const int STORY_POINT_PANEL_HEIGHT = 400;
        var storyPointRect = new Rectangle(
            contentRect.Right - STORY_POINT_PANEL_WIDTH - ControlBase.CONTENT_MARGIN,
            contentRect.Top + ControlBase.CONTENT_MARGIN,
            STORY_POINT_PANEL_WIDTH,
            STORY_POINT_PANEL_HEIGHT);

        DrawCurrentStoryPoints(spriteBatch, storyPointRect);

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
            _campaignMap.Update(ResolutionIndependentRenderer);

            UpdateMapPresentation(gameTime, _campaignMap);
        }

        _showStoryPointsButton.Update(ResolutionIndependentRenderer);
    }

    private void DrawCurrentStoryPoints(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        if (!_globe.Globe.ActiveStoryPoints.Any())
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
        }
    }

    private void InitializeCampaignItemButtons()
    {
        var currentCampaign = _screenTransitionArguments.Campaign;

        _campaignMap = new CampaignMap(currentCampaign, ScreenManager, this,
            Game.Content.Load<Texture2D>("Sprites/Ui/CampaignStageIcons"),
            ResolutionIndependentRenderer);
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

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            _presentationDelayCounter = 0;
            _isCampaignPresentation = false;
            campaignMap.State = CampaignMap.MapState.Interactive;

            campaignMap.Scroll = campaignMap.StartScroll;
        }

        if (_presentationDelayCounter > 0)
        {
            _presentationDelayCounter -= gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            if ((campaignMap.Scroll - campaignMap.StartScroll).Length() > 10)
            {
                campaignMap.Scroll = Vector2.Lerp(campaignMap.Scroll, campaignMap.StartScroll,
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