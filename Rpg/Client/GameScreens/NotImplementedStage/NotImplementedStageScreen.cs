using System;
using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.GameScreens.Campaign;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.NotImplementedStage;

internal class NotImplementedStageScreen : GameScreenWithMenuBase
{
    private readonly HeroCampaign _campaign;

    public NotImplementedStageScreen(TestamentGame game, NotImplementedStageScreenTransitionArguments args) : base(game)
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

        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
            new CampaignScreenTransitionArguments(_campaign));
    }
}