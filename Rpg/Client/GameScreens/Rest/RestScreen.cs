using System;
using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.GameScreens.Campaign;
using Client.GameScreens.Rest.Ui;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Rest;

internal sealed class RestScreen : GameScreenWithMenuBase
{
    private readonly IList<ButtonBase> _actionButtons;

    private readonly HeroCampaign _campaign;
    private readonly IUiContentStorage _uiContentStorage;

    public RestScreen(TestamentGame game, RestScreenTransitionArguments args) : base(game)
    {
        _campaign = args.Campaign;

        _actionButtons = new List<ButtonBase>();

        _uiContentStorage = Game.Services.GetRequiredService<IUiContentStorage>();
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
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

        const int ACTION_BUTTON_WIDTH = 200;
        const int ACTION_BUTTON_HEIGHT = 40;

        const int HEADER_HEIGHT = 100;

        spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), UiResource.RestScreen_Title,
            new Vector2(contentRect.Center.X, contentRect.Top + ControlBase.CONTENT_MARGIN), Color.Wheat);

        for (var buttonIndex = 0; buttonIndex < _actionButtons.Count; buttonIndex++)
        {
            var actionButton = _actionButtons[buttonIndex];
            actionButton.Rect = new Rectangle(
                contentRect.Center.X - ACTION_BUTTON_WIDTH / 2,
                HEADER_HEIGHT + buttonIndex * ACTION_BUTTON_HEIGHT,
                ACTION_BUTTON_WIDTH,
                ACTION_BUTTON_HEIGHT
            );

            actionButton.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        var improvedRestActionButton = new ResourceTextButton(nameof(UiResource.RestActionImprovedRest));
        _actionButtons.Add(improvedRestActionButton);

        var scoutingActionButton = new ResourceTextButton(nameof(UiResource.RestActionScouting));
        _actionButtons.Add(scoutingActionButton);

        var chatActionButton = new ResourceTextButton(nameof(UiResource.RestActionChat));
        _actionButtons.Add(chatActionButton);

        foreach (var actionButton in _actionButtons)
        {
            actionButton.OnClick += (_, _) =>
            {
                var underConstructionModal = new UnderConstructionModal(
                    _uiContentStorage,
                    ResolutionIndependentRenderer);

                underConstructionModal.Closed += (_, _) =>
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
                        new CampaignScreenTransitionArguments(_campaign));
                };

                AddModal(underConstructionModal, false);
                _campaign.CompleteCurrentStage();
            };
        }
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        foreach (var actionButton in _actionButtons)
        {
            actionButton.Update(ResolutionIndependentRenderer);
        }
    }
}