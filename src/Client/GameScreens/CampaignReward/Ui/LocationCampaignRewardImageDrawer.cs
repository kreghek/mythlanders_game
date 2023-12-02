using System.Collections.Generic;

using Client.Assets;
using Client.Assets.Catalogs;
using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward.Ui;

internal sealed class HeroCampaignRewardImageDrawer : CampaignRewardImageDrawerBase<HeroCampaignReward>
{
    private ContentManager _content;
    private readonly ICombatantGraphicsCatalog _unitGraphicsCatalog;
    private Texture2D? _thumbnailIcon;

    public override Point ImageSize => new(32, 32);

    public HeroCampaignRewardImageDrawer(ContentManager content, ICombatantGraphicsCatalog unitGraphicsCatalog)
    {
        _content = content;
        _unitGraphicsCatalog = unitGraphicsCatalog;
    }

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

    public override Point ImageSize => new(200, 100);
}
