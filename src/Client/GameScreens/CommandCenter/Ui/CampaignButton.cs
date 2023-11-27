using System.Collections.Generic;

using Client.Core;
using Client.Engine;
using Client.GameScreens.CampaignReward;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CommandCenter.Ui;

internal sealed class CampaignButton : ButtonBase
{
    private readonly Texture2D _campaignTexture;
    private readonly IReadOnlyCollection<ICampaignReward> _estimatedRewards;

    public CampaignButton(Texture2D campaignTexture, ILocationSid location,
        IReadOnlyCollection<ICampaignReward> estimatedRewards)
    {
        _campaignTexture = campaignTexture;
        _estimatedRewards = estimatedRewards;
        Location = location;
    }

    public bool Hover { get; set; }
    public ILocationSid Location { get; }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.CombatMove;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.Draw(_campaignTexture,
            new Rectangle(
                contentRect.Left + CONTENT_MARGIN,
                contentRect.Top + CONTENT_MARGIN,
                contentRect.Width - (CONTENT_MARGIN * 2),
                contentRect.Height - (CONTENT_MARGIN * 2) - 20),
            new Rectangle(0, Hover ? 0 : 100, 200, Hover ? 200 : 100),
            Color.White);

        spriteBatch.DrawString(
            UiThemeManager.UiContentStorage.GetTitlesFont(),
            GameObjectHelper.GetLocalized(Location),
            new Vector2(contentRect.Left + CONTENT_MARGIN, contentRect.Bottom - CONTENT_MARGIN - 20),
            Color.Wheat);

        foreach (var estimatedReward in _estimatedRewards)
        {
            spriteBatch.DrawString(
                UiThemeManager.UiContentStorage.GetMainFont(),
                estimatedReward.GetRewardDescription(),
                new Vector2(contentRect.Left + CONTENT_MARGIN, contentRect.Bottom - CONTENT_MARGIN - 20 + 20 + CONTENT_MARGIN),
                Color.Wheat);
        }
    }
}