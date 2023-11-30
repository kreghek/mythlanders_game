using System.Collections.Generic;
using System.Linq;

using Client.Assets;
using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward.Ui;

internal sealed class LocationCampaignRewardImageDrawer: CampaignRewardImageDrawerBase<LocationCampaignReward>
{
    private readonly ContentManager _content;
    private readonly IDictionary<ILocationSid, Texture2D> _locationTextures;

    public LocationCampaignRewardImageDrawer(ContentManager content)
    {
        _content = content;
        
        _locationTextures = new Dictionary<ILocationSid, Texture2D>
        {
            { LocationSids.Desert, LoadCampaignThumbnailImage("Desert") },
            { LocationSids.Monastery, LoadCampaignThumbnailImage("Monastery") },
            { LocationSids.ShipGraveyard, LoadCampaignThumbnailImage("ShipGraveyard") },
            { LocationSids.Thicket, LoadCampaignThumbnailImage("DarkThicket") },
            { LocationSids.Swamp, LoadCampaignThumbnailImage("GrimSwamp") },
            { LocationSids.Battleground, LoadCampaignThumbnailImage("Battleground") }
        };
    }
    
    Texture2D LoadCampaignThumbnailImage(string textureName)
    {
        return _content.Load<Texture2D>($"Sprites/GameObjects/Campaigns/{textureName}");
    }

    protected override void Draw(LocationCampaignReward reward, SpriteBatch spriteBatch, Vector2 position)
    {
        var texture = _locationTextures[reward.Location];
        spriteBatch.Draw(texture, position, new Rectangle(50,0,200,100), Color.White);
    }

    public override Point ImageSize => new Point(200, 100);
}

internal sealed class RewardPanel: ControlBase
{
    private readonly IReadOnlyCollection<ICampaignReward> _rewards;
    private readonly Texture2D _panelHeaderTexture;
    private readonly SpriteFont _labelFont;
    private readonly SpriteFont _rewardNameFont;
    private readonly IReadOnlyCollection<ICampaignRewardImageDrawer> _rewardImageDrawers;

    public RewardPanel(
        IReadOnlyCollection<ICampaignReward> rewards,
        Texture2D panelHeaderTexture,
        SpriteFont labelFont,
        SpriteFont rewardNameFont,
        IReadOnlyCollection<ICampaignRewardImageDrawer> rewardImageDrawers)
    {
        _rewards = rewards;
        _panelHeaderTexture = panelHeaderTexture;
        _labelFont = labelFont;
        _rewardNameFont = rewardNameFont;
        _rewardImageDrawers = rewardImageDrawers;
    }
    
    protected override Point CalcTextureOffset() => ControlTextures.Panel;

    protected override Color CalculateColor() => Color.White;

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        DrawPanelHeader(spriteBatch, contentRect);

        DrawRewardLabel(spriteBatch, contentRect);

        var rewards = _rewards.ToArray();
        for (var i = 0; i < rewards.Length; i++)
        {
            var reward = rewards[i];
            var rewardPosition = (contentRect.Location + new Point(contentRect.Center.X, i * 32)).ToVector2();

            foreach (var campaignRewardImageDrawer in _rewardImageDrawers)
            {
                if (!campaignRewardImageDrawer.IsApplicable(reward))
                {
                    continue;
                }

                var imageSize = campaignRewardImageDrawer.ImageSize;
                campaignRewardImageDrawer.Draw(reward, spriteBatch, rewardPosition - new Vector2(imageSize.X, 0));
            }
            
            spriteBatch.DrawString(_rewardNameFont,
                reward.GetRewardName(),
                rewardPosition, Color.White);
        }
    }

    private void DrawRewardLabel(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        var labelSize = _labelFont.MeasureString(UiResource.CampaignRewardsLabelText);
        spriteBatch.DrawString(_labelFont, UiResource.CampaignRewardsLabelText,
            new Vector2(contentRect.Center.X - labelSize.X / 2, contentRect.Top + 50), Color.Wheat);
    }

    private void DrawPanelHeader(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        spriteBatch.Draw(_panelHeaderTexture,
            new Vector2(
                contentRect.Center.X - _panelHeaderTexture.Width / 2,
                contentRect.Top - _panelHeaderTexture.Height / 2),
            Color.White);
    }
}