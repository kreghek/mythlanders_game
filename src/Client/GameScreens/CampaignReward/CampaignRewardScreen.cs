using System;
using System.Collections.Generic;

using Client.Assets.Catalogs;
using Client.Core;
using Client.Core.CampaignRewards;
using Client.Engine;
using Client.GameScreens.CampaignReward.Ui;
using Client.GameScreens.CommandCenter;
using Client.ScreenManagement;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward;

internal sealed class CampaignRewardScreen : GameScreenWithMenuBase
{
    private readonly ICampaignGenerator _campaignGenerator;
    private readonly GlobeProvider _globeProvider;
    private readonly ResourceTextButton _moveNextButton;
    private readonly IReadOnlyCollection<ICampaignReward> _rewards;
    private readonly IUiContentStorage _uiContent;
    private RewardPanel _rewardPanel = null!;

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

        _rewardPanel.Rect = new Rectangle(contentRect.Center.X - 300, contentRect.Top + 20, 600, 400);
        _rewardPanel.Draw(spriteBatch);

        _moveNextButton.Rect = new Rectangle(new Point(_rewardPanel.Rect.Location.X, _rewardPanel.Rect.Bottom),
            new Point(100, 20));
        _moveNextButton.Draw(spriteBatch);

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        var panelHeaderTexture = Game.Content.Load<Texture2D>("Sprites/Ui/CombatSkillsPanel");
        _rewardPanel = new RewardPanel(_rewards, panelHeaderTexture, _uiContent.GetTitlesFont(),
            _uiContent.GetTitlesFont(), new ICampaignRewardImageDrawer[]
            {
                new PropCampaignRewardImageDrawer(Game.Content.Load<Texture2D>("Sprites/GameObjects/EquipmentIcons")),
                new LocationCampaignRewardImageDrawer(Game.Content),
                new HeroCampaignRewardImageDrawer(Game.Content,
                    Game.Services.GetRequiredService<ICombatantGraphicsCatalog>())
            });
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