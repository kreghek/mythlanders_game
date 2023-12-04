using Client.Core.CampaignRewards;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward.Ui;

internal sealed class PropCampaignRewardImageDrawer: CampaignRewardImageDrawerBase<ResourceCampaignReward>
{
    private readonly Texture2D _propTexture;

    public PropCampaignRewardImageDrawer(Texture2D propTexture)
    {
        _propTexture = propTexture;
    }

    protected override void Draw(ResourceCampaignReward reward, SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(_propTexture, position, new Rectangle(0,0,32,32), Color.White);
    }

    public override Point ImageSize => new Point(32);
}