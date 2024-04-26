using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.CampaignEffects;
using Client.Engine;
using Client.GameScreens.CampaignReward.Ui;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.Result;

internal sealed class RewardPanel : ControlBase
{
    private readonly SpriteFont _labelFont;
    private readonly Texture2D _panelHeaderTexture;
    private readonly IReadOnlyCollection<ICampaignRewardImageDrawer> _rewardImageDrawers;
    private readonly SpriteFont _rewardNameFont;
    private readonly IReadOnlyCollection<ICampaignEffect> _rewards;

    public RewardPanel(
        IReadOnlyCollection<ICampaignEffect> rewards,
        Texture2D panelHeaderTexture,
        SpriteFont labelFont,
        SpriteFont rewardNameFont,
        IReadOnlyCollection<ICampaignRewardImageDrawer> rewardImageDrawers) : base(UiThemeManager.UiContentStorage
        .GetControlBackgroundTexture())
    {
        _rewards = rewards;
        _panelHeaderTexture = panelHeaderTexture;
        _labelFont = labelFont;
        _rewardNameFont = rewardNameFont;
        _rewardImageDrawers = rewardImageDrawers;
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.Panel;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        DrawPanelHeader(spriteBatch, contentRect);

        DrawRewardLabel(spriteBatch, contentRect);

        var rewards = _rewards.ToArray();
        for (var i = 0; i < rewards.Length; i++)
        {
            var reward = rewards[i];
            var rewardPosition = (new Point(contentRect.Center.X, contentRect.Top + 50 + 50 + i * 32)).ToVector2();

            foreach (var campaignRewardImageDrawer in _rewardImageDrawers)
            {
                if (!campaignRewardImageDrawer.IsApplicable(reward))
                {
                    continue;
                }

                var imageSize = campaignRewardImageDrawer.ImageSize;
                campaignRewardImageDrawer.Draw(reward, spriteBatch, rewardPosition - new Vector2(imageSize.X, 0));
            }
        }
    }

    private void DrawPanelHeader(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        spriteBatch.Draw(_panelHeaderTexture,
            new Vector2(
                contentRect.Center.X - _panelHeaderTexture.Width / 2,
                contentRect.Top - _panelHeaderTexture.Height / 2),
            Color.White);
    }

    private void DrawRewardLabel(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        var labelSize = _labelFont.MeasureString(UiResource.CampaignRewardsLabelText);
        spriteBatch.DrawString(_labelFont, UiResource.CampaignRewardsLabelText,
            new Vector2(contentRect.Center.X - labelSize.X / 2, contentRect.Top + 50), Color.Wheat);
    }

    public void Update(GameTime gameTime)
    {
        foreach (var campaignRewardImageDrawer in _rewardImageDrawers)
        {
            campaignRewardImageDrawer.Update(gameTime);
        }
    }
}