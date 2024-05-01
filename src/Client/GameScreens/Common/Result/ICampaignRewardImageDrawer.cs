using Client.Core.CampaignEffects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward.Ui;

internal interface ICampaignRewardImageDrawer
{
    Point ImageSize { get; }

    void Draw(ICampaignEffect reward, SpriteBatch spriteBatch, Vector2 position);

    bool IsApplicable(ICampaignEffect reward);

    public void Update(GameTime gameTime);
}