using Client.Core.CampaignEffects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward.Ui;

internal abstract class CampaignRewardImageDrawerBase<TReward> : ICampaignRewardImageDrawer
    where TReward : ICampaignEffect
{
    protected abstract void Draw(TReward reward, SpriteBatch spriteBatch, Vector2 position);

    public abstract Point ImageSize { get; }

    public void Draw(ICampaignEffect reward, SpriteBatch spriteBatch, Vector2 position)
    {
        Draw((TReward)reward, spriteBatch, position);
    }

    public bool IsApplicable(ICampaignEffect reward)
    {
        return reward is TReward;
    }

    public virtual void Update(GameTime gameTime)
    {
    }
}