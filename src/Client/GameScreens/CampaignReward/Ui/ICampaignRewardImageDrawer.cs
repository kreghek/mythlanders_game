using Client.Core.CampaignRewards;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward.Ui;

internal interface ICampaignRewardImageDrawer
{
    Point ImageSize { get; }

    void Draw(ICampaignReward reward, SpriteBatch spriteBatch, Vector2 position);

    bool IsApplicable(ICampaignReward reward);
}