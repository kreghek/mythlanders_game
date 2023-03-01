using System;
using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.GameScreens.Campaign;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.NotImplementedStage;

internal class NotImplementedStageScreen : GameScreenWithMenuBase
{
    private readonly HeroCampaign _campaign;
    private readonly IUiContentStorage _uiContentStorage;

    public NotImplementedStageScreen(TestamentGame game, NotImplementedStageScreenTransitionArguments args) : base(game)
    {
        _campaign = args.Campaign;

        _uiContentStorage = Game.Services.GetRequiredService<IUiContentStorage>();
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        var closeButton = new TextButton("Skip");
        closeButton.OnClick += CloseButton_OnClick;

        return new[]
        {
            closeButton
        };
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), "Under construction\n\n(press Skip to continue)", contentRect.Center.ToVector2(), Color.Wheat);

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        _campaign.CompleteCurrentStage();

        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
            new CampaignScreenTransitionArguments(_campaign));
    }
}