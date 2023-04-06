using System;
using System.Collections.Generic;
using System.Linq;

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
    (HeroCampaign Campaign, IReadOnlyCollection<IProp> CampaignRewards) : CampaignScreenTransitionArgumentsBase(
        Campaign);

internal sealed class CampaignRewardScreen : GameScreenWithMenuBase
{
    private readonly ICampaignGenerator _campaignGenerator;
    private readonly ResourceTextButton _moveNextButton;
    private readonly IReadOnlyCollection<IProp> _reward;
    private readonly IUiContentStorage _uiContent;

    public CampaignRewardScreen(TestamentGame game, CampaignRewardScreenTransitionArguments args) : base(game)
    {
        _campaignGenerator = game.Services.GetRequiredService<ICampaignGenerator>();
        _uiContent = game.Services.GetRequiredService<IUiContentStorage>();

        _moveNextButton = new ResourceTextButton(nameof(UiResource.CompleteCampaignButtonTitle));
        _moveNextButton.OnClick += MoveNextButton_OnClick;

        _reward = args.CampaignRewards;
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

        _moveNextButton.Rect = new Rectangle(contentRect.Location, new Point(100, 20));
        _moveNextButton.Draw(spriteBatch);

        var array = _reward.ToArray();
        for (var i = 0; i < array.Length; i++)
        {
            var prop = array[i];
            spriteBatch.DrawString(_uiContent.GetTitlesFont(), prop.Scheme.Sid,
                (contentRect.Location + new Point(0, i * 32)).ToVector2(), Color.White);
        }

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
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

    private void MoveNextButton_OnClick(object? sender, EventArgs e)
    {
        MoveNext();
    }
}