using System;
using System.Collections.Generic;

using Core.Minigames.Match3;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Campaign;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Match3;

internal class Match3MinigameScreen : GameScreenWithMenuBase
{
    private readonly HeroCampaign _campaign;

    public Match3MinigameScreen(EwarGame game, Match3MinigameScreenTransitionArguments args) : base(game)
    {
        _campaign = args.Campaign;

        _match3Engine = new Match3Engine
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
        ResolutionIndependentRenderer.BeginDraw();
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

       
        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        throw new NotImplementedException();
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        _campaign.CompleteCurrentStage();

        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign, new CampaignScreenTransitionArguments
        {
            Campaign = _campaign
        });
    }
}
