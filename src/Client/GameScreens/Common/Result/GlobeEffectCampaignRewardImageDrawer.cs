using Client.Core.CampaignEffects;
using Client.GameScreens.CampaignReward.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.Result;

internal sealed class GlobeEffectCampaignRewardImageDrawer : CampaignRewardImageDrawerBase<AddGlobalEffectCampaignEffect>
{
    private readonly SpriteFont _font;

    public GlobeEffectCampaignRewardImageDrawer(SpriteFont font)
    {
        _font = font;
    }

    public override Point ImageSize { get; }

    protected override void Draw(AddGlobalEffectCampaignEffect reward, SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.DrawString(_font, reward.TargetGlobeEvent.TitleSid, position, Color.White);
    }
}