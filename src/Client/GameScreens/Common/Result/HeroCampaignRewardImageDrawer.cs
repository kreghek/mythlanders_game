﻿using Client.Assets.Catalogs;
using Client.Core.CampaignEffects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward.Ui;

internal sealed class HeroCampaignRewardImageDrawer : CampaignRewardImageDrawerBase<UnlockHeroCampaignEffect>
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

    protected override void Draw(UnlockHeroCampaignEffect reward, SpriteBatch spriteBatch, Vector2 position)
    {
        if (_thumbnailIcon is null)
        {
            var classSid = reward.Hero;
            var thumbnailPath = _unitGraphicsCatalog.GetGraphics(classSid).ThumbnailPath;
            _thumbnailIcon = _content.Load<Texture2D>(thumbnailPath);
        }
    }
}