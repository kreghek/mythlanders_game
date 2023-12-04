using Client.Assets.Catalogs;
using Client.Core.CampaignRewards;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward.Ui;

internal sealed class HeroCampaignRewardImageDrawer : CampaignRewardImageDrawerBase<HeroCampaignReward>
{
    private readonly ContentManager _content;
    private readonly ICombatantGraphicsCatalog _unitGraphicsCatalog;
    private Texture2D? _thumbnailIcon;

    public HeroCampaignRewardImageDrawer(ContentManager content, ICombatantGraphicsCatalog unitGraphicsCatalog)
    {
        _content = content;
        _unitGraphicsCatalog = unitGraphicsCatalog;
    }

    public override Point ImageSize => new(32, 32);

    protected override void Draw(HeroCampaignReward reward, SpriteBatch spriteBatch, Vector2 position)
    {
        if (_thumbnailIcon is null)
        {
            var classSid = reward.Hero.ToString();
            var thumbnailPath = _unitGraphicsCatalog.GetGraphics(classSid).ThumbnailPath;
            _thumbnailIcon = _content.Load<Texture2D>(thumbnailPath);
        }
    }
}