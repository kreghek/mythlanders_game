using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Campaign;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.NotImplementedStage;

internal class NotImplementedStageScreen : GameScreenWithMenuBase
{
    private HeroCampaign _campaign;

    public NotImplementedStageScreen(EwarGame game, NotImplementedStageScreenTransitionArguments args) : base(game)
    {
        _campaign = args.Campaign;
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

    }

    protected override void InitializeContent()
    {

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