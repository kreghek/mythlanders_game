using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Engine;
using Client.GameScreens.CommandCenter;
using Client.ScreenManagement;

using Core.Props;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward;

internal sealed class ResourceCampaignReward : ICampaignReward
{
    private readonly IProp _resource;

    public ResourceCampaignReward(IProp resource)
    {
        _resource = resource;
    }

    public string GetRewardDescription()
    {
        return _resource.Scheme.Sid;
    }
}

internal sealed class CampaignRewardScreen : GameScreenWithMenuBase
{
    private readonly ICampaignGenerator _campaignGenerator;
    private readonly GlobeProvider _globeProvider;
    private readonly ResourceTextButton _moveNextButton;
    private readonly IReadOnlyCollection<ICampaignReward> _rewards;
    private readonly IUiContentStorage _uiContent;

    public CampaignRewardScreen(TestamentGame game, CampaignRewardScreenTransitionArguments args) : base(game)
    {
        _campaignGenerator = game.Services.GetRequiredService<ICampaignGenerator>();
        _uiContent = game.Services.GetRequiredService<IUiContentStorage>();
        _globeProvider = game.Services.GetRequiredService<GlobeProvider>();

        _moveNextButton = new ResourceTextButton(nameof(UiResource.CompleteCampaignButtonTitle));
        _moveNextButton.OnClick += MoveNextButton_OnClick;

        _rewards = args.CampaignRewards;
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

        var array = _rewards.ToArray();
        for (var i = 0; i < array.Length; i++)
        {
            var prop = array[i];
            spriteBatch.DrawString(_uiContent.GetTitlesFont(), prop.GetRewardDescription(),
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
        var campaigns = _campaignGenerator.CreateSet(_globeProvider.Globe);
        ScreenManager.ExecuteTransition(this, ScreenTransition.CommandCenter,
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