using System;
using System.Collections.Generic;
using System.Linq;

using Client.Engine;
using Client.GameScreens.Campaign.Ui;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Campaign;

internal class CampaignScreen : GameScreenWithMenuBase
{
    private readonly GlobeProvider _globe;
    private readonly CampaignScreenTransitionArguments _screenTransitionArguments;
    private readonly ButtonBase _showStoryPointsButton;
    private readonly ButtonBase _inventoryButton;

    private bool _showStoryPoints;
    private CampaignPanel? _stagePanel;

    public CampaignScreen(TestamentGame game, CampaignScreenTransitionArguments screenTransitionArguments) : base(game)
    {
        _screenTransitionArguments = screenTransitionArguments;

        _globe = game.Services.GetRequiredService<GlobeProvider>();

        _showStoryPointsButton = new ResourceTextButton(nameof(UiResource.CurrentQuestButtonTitle));
        _showStoryPointsButton.OnClick += ShowStoryPointsButton_OnClick;

        _inventoryButton = new ResourceTextButton(nameof(UiResource.InventoryButtonTitle));
        _inventoryButton.OnClick += InventoryButton_OnClick;
    }

    private void InventoryButton_OnClick(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return new[]
        {
            _inventoryButton
        };
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

        if (_stagePanel is not null)
        {
            _stagePanel.Rect = contentRect;
            _stagePanel.Draw(spriteBatch);
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

        if (_stagePanel is not null)
        {
            _stagePanel.Update(ResolutionIndependentRenderer);
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

        _stagePanel = new CampaignPanel(currentCampaign, ScreenManager, this);
    }

    private void ShowStoryPointsButton_OnClick(object? sender, EventArgs e)
    {
        _showStoryPoints = !_showStoryPoints;
    }
}