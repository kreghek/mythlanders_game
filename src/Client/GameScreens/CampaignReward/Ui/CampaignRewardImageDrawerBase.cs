using Client.Core.CampaignRewards;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward.Ui;

internal abstract class CampaignRewardImageDrawerBase<TReward>: ICampaignRewardImageDrawer where TReward : ICampaignReward
{
    protected abstract void Draw(TReward reward, SpriteBatch spriteBatch, Vector2 position);
    
    public abstract  Point ImageSize { get; }
    
    public void Draw(ICampaignReward reward, SpriteBatch spriteBatch, Vector2 position)
    {
        Draw((TReward)reward, spriteBatch, position);
    }

    public bool IsApplicable(ICampaignReward reward)
    {
        return reward is TReward;
    }
}