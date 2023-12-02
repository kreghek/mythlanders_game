using System.Collections.Generic;

using Client.Assets;
using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CampaignReward.Ui;

internal sealed class LocationCampaignRewardImageDrawer : CampaignRewardImageDrawerBase<LocationCampaignReward>
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

    public override Point ImageSize => new(200, 100);

    protected override void Draw(LocationCampaignReward reward, SpriteBatch spriteBatch, Vector2 position)
    {
        var texture = _locationTextures[reward.Location];
        spriteBatch.Draw(texture, position, new Rectangle(50, 0, 200, 100), Color.White);
    }

    private Texture2D LoadCampaignThumbnailImage(string textureName)
    {
        return _content.Load<Texture2D>($"Sprites/GameObjects/Campaigns/{textureName}");
    }
}