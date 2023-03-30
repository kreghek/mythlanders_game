using System;
using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.CommandCenter;

using Core.Props;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.CampaignReward;

internal sealed record CampaignRewardScreenTransitionArguments
    (HeroCampaign Campaign, IReadOnlyCollection<IProp> CampaignRewards) : CampaignScreenTransitionArgumentsBase(Campaign);

internal sealed class CampaignRewardScreen: GameScreenWithMenuBase
{
    private readonly ICampaignGenerator _campaignGenerator;
    private readonly ResourceTextButton _moveNextButton;

    public CampaignRewardScreen(TestamentGame game, CampaignRewardScreenTransitionArguments args) : base(game)
    {
        _campaignGenerator = game.Services.GetRequiredService<ICampaignGenerator>();

        _moveNextButton = new ResourceTextButton(nameof(UiResource.CompleteCampaignButtonTitle));
        _moveNextButton.OnClick += MoveNextButton_OnClick;
    }

    private void MoveNextButton_OnClick(object? sender, EventArgs e)
    {
        MoveNext();
    }

    protected override void InitializeContent()
    {
        
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        _moveNextButton.Rect = new Rectangle(contentRect.Location, new Point(100, 20));
        _moveNextButton.Draw(spriteBatch);
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);
        
        _moveNextButton.Update(ResolutionIndependentRenderer);
    }

    private void MoveNext()
    {
        var campaigns = _campaignGenerator.CreateSet();
        ScreenManager.ExecuteTransition(this, ScreenTransition.CampaignSelection,
            new CommandCenterScreenTransitionArguments
            {
                AvailableCampaigns = campaigns
            });
    }
}